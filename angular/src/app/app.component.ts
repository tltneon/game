import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit { 
    error:string;

    constructor(){}
      
    ngOnInit() {
        this.registerError();
    }
    registerError(err:string = "errno"){
      document.body.querySelector("#errorSign").classList.add("error");
      this.error = err;
    }
    clear(){
      document.body.querySelector("#errorSign").classList.remove("error");
      this.error = "";
    }
}