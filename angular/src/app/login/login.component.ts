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

  ngOnInit() {}

  auth(password, login){
    function updateErrorMessage(baseclass, str){
      baseclass.error = typeof(str) == "string" ? str : str.name + " ("+str.error.message+")";
      document.body.querySelector("#auth").innerHTML = "Authorize";
      document.body.querySelector("#processing").classList.remove("active", "progress");
    }

    function proceedAuth(router, responce){
      Cookie.set('token', responce);
      router.navigate(['/']);
    }

    this.error = ". . .";
    document.body.querySelector('#auth').innerHTML = "Processing...";
    document.body.querySelector("#processing").classList.add("active", "progress");
    this.httpService.postRequest("api/account/auth", {"username": login, "password": password}, false).subscribe(
      (responce:string) => responce.slice(0,5) == "Error" 
        ? updateErrorMessage(this, responce.replace('Error#',''))
        : proceedAuth(this.router, responce)
      , error => updateErrorMessage(this, error));
  }

  fakeauth(){
    Cookie.set('token', 'faketoken');
    this.router.navigate(['/']);
  }
}