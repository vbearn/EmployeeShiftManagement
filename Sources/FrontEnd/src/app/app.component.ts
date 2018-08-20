import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { Settings } from '../settings';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  totalEmployees: number = 10;
  totalDays: number = 14;
  holidaysOff: boolean = true;
  randomizeEmployees: boolean = true;

  firstShiftEmployee: number = 1;
  secondShiftEmployee: number = 2;

  calculatedDays: any[];

  constructor(
    private http: Http
  ) {


  }

  calculateClicked() {

    this.calculatedDays = [];

    let calculateModel = {
      TotalEmployees: this.totalEmployees,
      FirstShiftEmployee: this.firstShiftEmployee,
      SecondShiftEmployee: this.secondShiftEmployee,
      totalDays: this.totalDays,
      holidaysOff: this.holidaysOff,
      randomizeEmployees: this.randomizeEmployees,
    };

    this.http.post(Settings.GET_SERVICE_URL(), calculateModel).subscribe(result => {
      let resultJson = result.json();
      this.calculatedDays = resultJson.days;
    },
      error => {
        alert("Service connection error. Please check SERVICE_URL in settings.ts file.")
      });
  }
}
