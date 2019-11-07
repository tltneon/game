import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Cookie } from 'ng2-cookies/ng2-cookies';
  
@Injectable()
export class HttpService {
    readonly rootUrl = 'http://localhost:16462/'; // webapi url
  
    constructor(private http: HttpClient){ }
      
    getRequest(url: string, httpOptions = {}) {
        console.log("get", this.rootUrl + url);
        return this.http.get(this.rootUrl + url, httpOptions);
    }      
    postRequest(url: string, body: any, addToken:boolean = false, httpOptions = {}) {
        console.log(Cookie.get("Token"));
        if(addToken) body.token = Cookie.get("Token");
        console.log("post", this.rootUrl + url, body );
        return this.http.post(this.rootUrl + url, body, httpOptions);
    }
}