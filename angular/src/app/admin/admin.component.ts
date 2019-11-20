import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit {
  level = "users";

  menu = [
    {name: "Users", routerLink: "users"},
    {name: "Bases", routerLink: "bases"},
  ];

  constructor() { }

  ngOnInit() {}

  changeLayer(level: string): void {
    this.level = level;
  }
}