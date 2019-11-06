import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
  
@Injectable()
export class HttpService {
    readonly rootUrl = 'http://localhost:16462/'; // webapi url
  
    constructor(private http: HttpClient){ }
      
    getRequest(url: string, httpOptions = {}) {
        console.log("get", this.rootUrl + url);
        return this.http.get(this.rootUrl + url, httpOptions);
    }      
    postRequest(url: string, body: any, httpOptions = {}) {
        console.log("post", this.rootUrl + url, body );
        return this.http.post(this.rootUrl + url, body, httpOptions);
    }
}