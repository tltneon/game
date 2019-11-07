import { Component, OnInit } from '@angular/core';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {
  menu = [];
  error:string;

  constructor() { }

  ngOnInit() {
    this.registerError();
    if(Cookie.get('token'))
      this.menu = [
      {name: "Base", routerLink: "base"},
      {name: "Battles", routerLink: "battles"},
      {name: "Stats", routerLink: "stats"},
      {name: "Settings", routerLink: "settings"},
      {name: "Admin", routerLink: "admin"},
    ];
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