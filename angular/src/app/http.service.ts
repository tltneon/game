import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
  
@Injectable()
export class HttpService {
    readonly rootUrl = 'http://localhost:16462/';
  
    constructor(private http: HttpClient){ }
      
    getData(url: string, httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json', }), responseType: 'text' as 'json' }) {
        console.log("get", this.rootUrl + url);
        return this.http.get(this.rootUrl + url, httpOptions);
    }      
    sendData(url: string, body: any, httpOptions = {}) {
        console.log("post", this.rootUrl + url, body );
        return this.http.post(this.rootUrl + url, body, httpOptions);
    }
}