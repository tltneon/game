import { Component, OnInit } from '@angular/core';
import { HttpService } from '../http.service';
import { Router } from '@angular/router';
import { Cookie } from 'ng2-cookies/ng2-cookies';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers: [HttpService]
})
export class LoginComponent implements OnInit {
  password: string;
  login:string;
  error:string = "r u rly authed?";

  constructor(private httpService: HttpService, private router: Router) { }

  ngOnInit() {
  }

  auth(password, login){
    function updateErrorMessage(baseclass, str){
      console.log(str);
      baseclass.error = typeof(str) == "string" ? str : str.name + " ("+str.error.message+")";
      document.body.querySelector("#auth").innerHTML = "Authorize";
      document.body.querySelector("#processing").classList.remove("active");
    }
    function proceedAuth(router, responce){
      Cookie.set('token', responce);
      router.navigate(['/']);
    }

    this.error = ". . .";
    document.body.querySelector("#auth").innerHTML = "Processing...";
    document.body.querySelector("#processing").classList.add("active");
    this.httpService.postRequest("api/auth/post", {"username": login, "password": password}).subscribe(
      (responce:string) => responce == "notauthed" 
      ? updateErrorMessage(this, "Wrong Password")
      : proceedAuth(this.router, responce)
      , error => updateErrorMessage(this, error));
  }
  fakeauth(){
    Cookie.set('token', "fakecock");
    this.router.navigate(['/']);
  }
}