import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/http.service';

@Component({
  selector: 'app-admin-bases',
  templateUrl: './admin-bases.component.html',
  styleUrls: ['./admin-bases.component.css'],
  providers: [HttpService]
})
export class AdminBasesComponent implements OnInit {
  data;
  curItem: number = -1;

  constructor(private httpService: HttpService) {}

  ngOnInit() {
    this.httpService.getRequest('api/base/GetBaseList').subscribe(
      (responce) => this.data = responce,
      error => console.log(error.message));
  }

  showBase(num: number): void {
    this.curItem = num;
  }
}