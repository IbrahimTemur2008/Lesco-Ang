import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, HttpClientModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginObj: Login;

  constructor(private http: HttpClient, private router: Router) {
    this.loginObj = new Login();
  }

  onLogin() {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    const body = {
      UserName: this.loginObj.UserName,
      Password: this.loginObj.Password
    };

    this.http.post('http://localhost/WebAPIs/eMeterAPI.svc/SignIn', body, { headers })
      .subscribe(
        (response: any) => {
          if (response.Code === 0) {
            alert('Sign-in successful');
            this.router.navigateByUrl('/dashboard')
            this.router.navigate(['/home']);
          } else {
            alert(response.Message || 'Error occurred');
          }
        },
        (error) => {
          console.error('Error occurred:', error);
          alert('An error occurred while signing in.');
        }
      );
  }
}

export class Login { 
  UserName: string;
  Password: string;

  constructor() {
    this.UserName = '';
    this.Password = '';
  } 
}
