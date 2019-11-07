import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../http.service';
import { StatsJSON } from '../../models/stats';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css'],
  providers: [HttpService]
})
export class AdminUsersComponent implements OnInit {
  data: StatsJSON[] = [];

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.httpService.getRequest('api/Statistic/GetUserList').subscribe(
      (responce: StatsJSON[]) => this.data = responce,
      error => console.log(error.message));

    /*for(let i = 0; i<29;i++)
      this.data[this.data.length] = {
        name: "username"+i,
        base: "basename"+i,
        level: 1,
      };*/
  }

}