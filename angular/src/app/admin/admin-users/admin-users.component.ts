import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css'],
  providers: [HttpService]
})
export class AdminUsersComponent implements OnInit {
  data;
  curItem: number = -1;

  constructor(private httpService: HttpService) {}

  ngOnInit() {
    this.httpService.getRequest('api/Statistic/GetPlayerList').subscribe(
      (responce) => this.data = responce,
      error => console.log(error.message));
  }

  showUser(num: number): void {
    this.curItem = num;
  }
}