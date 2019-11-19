import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Router } from '@angular/router';
import { Cookie } from 'ng2-cookies/ng2-cookies';
import { GameVars } from '../gamevars';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [HttpService, GameVars]
})
export class LoginComponent implements OnInit {
  password: string;
  login:string;
  error:string = "Enter your credentials here:";

  constructor(private httpService: HttpService, private router: Router, private gameVars: GameVars) { }

  ngOnInit() {}

  auth(password, login){
    this.error = ". . .";
    document.body.querySelector('#auth').innerHTML = "Processing...";
    document.body.querySelector("#processing").classList.add("active", "progress");
    this.httpService.postRequest("api/account/auth", {"username": login, "password": password}, false).subscribe(
      (responce:string) => {
        if(responce.slice(0,5) == "Token") {
          Cookie.set('token', responce);
          this.router.navigate(['/']);
        }
        else {
          this.error = responce;
          document.body.querySelector("#auth").innerHTML = "Authorize";
          document.body.querySelector("#processing").classList.remove("active", "progress");
          this.gameVars.registerError(responce);
        }
      },
      error => this.gameVars.registerError(error.message));
  }

  fakeauth() {
    Cookie.set('token', 'faketoken');
    this.router.navigate(['/']);
  }
}