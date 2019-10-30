import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css']
})
export class AdminUsersComponent implements OnInit {
  data = [];
  constructor() { }

  ngOnInit() {
    for(let i = 0; i<29;i++)
      this.data[this.data.length] = {
        name: "username"+i,
        base: "basename"+i,
        level: 1,
      };
  }

}