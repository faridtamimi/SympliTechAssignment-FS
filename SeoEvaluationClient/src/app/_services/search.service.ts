import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RankingResult } from '../_models/ranking-result.model';
import { SearchInput } from '../_models/search-input.model';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  constructor(private http: HttpClient) { }

  search(model: SearchInput) : Observable<RankingResult[]>{
    return this.http.post<any>(environment.apiUrl + 'Search', model)
  }

  getSearchEngines() : Observable<string[]> {
      return this.http.get<string[]>(environment.apiUrl + 'Search/GetSearchEngineNames');
  }
}
