import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Router } from '@angular/router';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { GameVars } from '../gamevars';
import { AccountJSON } from '../models/account';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [HttpService, GameVars]
})
export class LoginComponent implements OnInit {
  password: string;
  login: string;
  error: string = "Enter your credentials here:";

  constructor(private httpService: HttpService, private router: Router, private gameVars: GameVars) { }

  ngOnInit() {}

  auth(password: string, login: string): void {
    this.error = ". . .";
    document.body.querySelector('#auth').innerHTML = "Processing...";
    document.body.querySelector("#processing").classList.add("active", "progress");
    this.httpService.postRequest("api/account/auth", {"username": login, "password": password}, false).subscribe(
      (responce: AccountJSON) => {
        if(responce.success){
          Cookie.set('token', responce.token);
          switch(responce.role){
            case 1: Cookie.set('role', "#M)R(IEFN*"); break;
            default: break;
          }
          Cookie.set('username', login);
          this.router.navigate(['/']);
        }
        else {
          document.body.querySelector("#auth").innerHTML = "Authorize";
          document.body.querySelector("#processing").classList.remove("active", "progress");
          this.error = this.gameVars.getText(responce.message);
        }
      },
      error => this.gameVars.registerError(error.message));
  }

  fakeauth(): void {
    Cookie.set('token', 'faketoken');
    this.router.navigate(['/']);
  }
}