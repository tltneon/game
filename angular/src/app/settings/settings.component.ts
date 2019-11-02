import { Component, OnInit, Input } from '@angular/core';
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
  password: string = '2341232';

  constructor(private httpService: HttpService) { }

  ngOnInit() {

    //console.log(this.input.nativeElement.value);
    this.httpService.getData('Test/Get').subscribe((responce:string) => this.userData = JSON.parse(responce));
    //, error => console.error(error));
  }

  setValue(password){
    this.password = password;
  }
}