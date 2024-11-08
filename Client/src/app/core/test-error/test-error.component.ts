import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss'],
})
export class TestErrorComponent {
  baseUrl = environment.apiUrl;
  validationErrors: string[] = [];

  constructor(private http: HttpClient) {}

  get404Erorr() {
    this.http.get(this.baseUrl + 'products/44').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get500Erorr() {
    this.http.get(this.baseUrl + 'buggy/servererorr').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get400Erorr() {
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error),
    });
  }
  get400ValidationErorr() {
    this.http.get(this.baseUrl + 'products/fortytwo').subscribe({
      next: (response) => console.log(response),
      error: (error) => {
        console.log(error);
        this.validationErrors = error.errors;
      },
    });
  }
}
