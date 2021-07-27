import { Component, OnInit } from '@angular/core';
import { SearchService } from '../_services/search.service';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SearchInput } from '../_models/search-input.model';
import { RankingResult } from '../_models/ranking-result.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  searchEngines: string[] = [];
  dropdownSettings = {};
  dropdownList: any[] = [];
  selectedItems: any[] = [];
  searchForm: FormGroup;
  rankingResults: RankingResult[] = [];
  loading = false;

  constructor(private searchService: SearchService, private formBuilder: FormBuilder, private toastrService: ToastrService) {
    this.searchForm = this.formBuilder.group({
      keywords: ['e-settlements'],
      url: ['www.sympli.com.au'],
      numberofresults: [10],
      selectedengines: [this.selectedItems]
    });
  }


  ngOnInit(): void {
    this.searchService.getSearchEngines().subscribe(x => {
      this.searchEngines = x;
      this.dropdownList = x.map(e => { return { item_id: e, item_text: e } });
      this.selectedItems = [];
    });

    this.dropdownSettings = {
      singleSelection: false,
      idField: 'item_id',
      textField: 'item_text',
      selectAllText: 'Select All',
      unSelectAllText: 'UnSelect All',
      itemsShowLimit: 3,
      allowSearchFilter: false
    };
  }

  onSubmit() {
    const searchenginevalue: any[] = this.searchForm.controls.selectedengines.value;
    
    if (searchenginevalue.length == 0) {
      this.toastrService.warning('Please selece at least one search engine', '', {
        timeOut: 3000,
      });
      return;
    }

    const numberofresults = parseInt(this.searchForm.controls.numberofresults.value);

    if (isNaN(numberofresults)) {
      this.toastrService.warning('Please provide a valid number of results', '', {
        timeOut: 3000,
      });
      return;
    }

    this.loading = true;
    const model: SearchInput = {
      keywords: this.searchForm.controls.keywords.value,
      numberOfResults: this.searchForm.controls.numberofresults.value,
      url: this.searchForm.controls.url.value,
      searchEngines: searchenginevalue.map(j => { return j.item_id })
    };
    this.searchService.search(model).subscribe(x => {
      this.loading = false;
      this.rankingResults = x;
    }, error => {
      let errormessage = '';
      const errorlist = error.error.errors;
      for (const element in errorlist) {
        console.log(element);
        errormessage += errorlist[element].join('<br/>') + '<br/>';

      }
      this.toastrService.warning(errormessage, '', {
        timeOut: 3000,
        enableHtml : true
      });
      this.loading = false;
    });
  }



}
