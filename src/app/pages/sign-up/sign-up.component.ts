import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, HttpClientModule],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css'
})
export class SignUpComponent implements OnInit {

  signUpForm: FormGroup;
  signUp: SignUp;

  constructor(private http: HttpClient,
    private router: Router,
    private formbuilder: FormBuilder) {
    this.signUp = new SignUp();
    this.signUpForm = this.formbuilder.group({
      userName: ['', [Validators.required]],
      cnic: ['', [Validators.required]],
      phoneNumber: ['', [Validators.required]],
      email: ['', [Validators.required]],
      password: ['', [Validators.required]],
    });
  }
  
  ngOnInit(): void {
  }

  onSignUp() {
    if (this.signUpForm.valid) {
      this.signUp = this.signUpForm.value;
      this.http.post('http://localhost:', this.signUp).subscribe((res:any)=>{
        if(res.result) {
          alert("You have signed up successfully!");
          this.router.navigateByUrl('/login');
        }
      });
    }
  }
}

export class SignUp { 
  email: string;
  password: string;
  cnic: string;
  phoneNmber: string;
  userName: string;
  constructor() {
    this.email = '';
    this.password = '';
    this.cnic = '';
    this.phoneNmber = '';
    this.userName = '';
  } 
}