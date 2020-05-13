import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/Alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
  }
login(){
 this.authService.login(this.model).subscribe(next => {
   console.log('login successfull');
   this.alertify.success('login successfull');
 }, error => {
   console.log(error) ;
   this.alertify.error('login Failed'); });
  }

loggedIn()
{
  return this.authService.loggedIn();
}

logout()
{
  localStorage.removeItem('token');
  this.alertify.message('logged out');

}
}
