import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit { 
    constructor(){}
      
    ngOnInit() {}

    clear(){
      document.body.querySelector("#errorSign").classList.remove("error");
      document.body.querySelector("#errorSign").children[1].children[1].innerHTML = '';
    }
}