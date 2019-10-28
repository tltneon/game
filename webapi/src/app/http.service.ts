import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
  
@Injectable()
export class HttpService {
    readonly rootUrl = 'http://localhost:16462/';
  
    constructor(private http: HttpClient){ }
      
    getData(url: string){
        return this.http.get(this.rootUrl + url);
    }      
    sendData(url: string, body: object){
        return this.http.post(this.rootUrl + url, body);
    }
    testRequest(){
        return this.http.get('http://localhost/testdata.json');
    }
}