import { Component, OnInit } from '@angular/core';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {
  menu = [];

  constructor() { }

  ngOnInit() {
    if(Cookie.get('token'))
      this.menu = [
      {name: "Base", routerLink: "base"},
      {name: "Battles", routerLink: "battles"},
      {name: "Stats", routerLink: "stats"},
      {name: "Settings", routerLink: "settings"},
      {name: "Admin", routerLink: "admin"},
    ];
  }
}