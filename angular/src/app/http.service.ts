import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
  
@Injectable()
export class HttpService {
    readonly rootUrl = 'http://localhost:16462/';
  
    constructor(private http: HttpClient){ }
      
    public getData(url: string) {
        return this.http.get(this.rootUrl + url, { withCredentials: true });
    }      
    sendData(url: string, body: object) {
        return this.http.post(this.rootUrl + url, body);
    }
    
    testRequest(url:string = 'http://localhost/testdata.json'){
        let httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', }), responseType: 'text' as 'json' };
        return this.http.get(url, httpOptions);
    }
}