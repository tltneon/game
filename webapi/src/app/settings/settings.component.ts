import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { UserJSON } from '../models/user';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: [HttpService]
})


export class SettingsComponent implements OnInit {
  userData: UserJSON;

  constructor(private httpService: HttpService) { }

  ngOnInit() {
    this.httpService.testRequest('http://localhost:16462/Test/Get').subscribe(responce => this.userData = JSON.parse(responce));
    //this.datas = JSON.parse(this.datas);
    //this.datas = this.httpService.getData("Test/Get");
    //console.log(this.datas);
    //this.httpService.testRequest().subscribe((results) => {this.datas = results;
    //console.log(this.datas);});
    //console.log(this.datas);
    //this.datas = JSON.parse(this.datas);
    //this.httpService.testRequest().subscribe((data: UserJSON) => this.userData = data, error => console.error(error));
    //.subscribe(data => this.userData = data, error => console.error(error));
  }
}