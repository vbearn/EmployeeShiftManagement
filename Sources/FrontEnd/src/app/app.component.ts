import { Component } from '@angular/core';
import { Http } from '../../node_modules/@angular/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  totalEmployees: number = 10;
  firstShiftEmployee: number = 1;
  secondShiftEmployee: number = 2;

  calculatedDays: any[];

  constructor(
    private http: Http
  ) {


  }

  calculateClicked() {

    this.calculatedDays = [];
    
    this.http.post("http://localhost:10104/api/schedule", {
      TotalEmployees: this.totalEmployees,
      FirstShiftEmployee: this.firstShiftEmployee,
      SecondShiftEmployee: this.secondShiftEmployee,
    }).subscribe(result => {
      let resultJson = result.json();
      this.calculatedDays = resultJson.days;
    });
  }
}
