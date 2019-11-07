import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  menu = [
    {name: "Users", routerLink: "users"},
    {name: "Bases", routerLink: "bases"},
    {name: "Settings", routerLink: "settings"},
  ];

  constructor() { }

  ngOnInit() {
  }

}