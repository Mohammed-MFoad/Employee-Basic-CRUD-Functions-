import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { throwError } from 'rxjs';
import { first } from 'rxjs/operators';
import { EmployeesController } from './core/APIs/EmployeesController';
import { Employee } from './core/models/Employee';
import { EmployeeService } from './core/services/employee.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Employee-UI';
  employees: any;
  employeeForm: boolean = false;
  isNewEmployee: boolean = false;
  newEmployee: any = {};
  editEmployeeForm: boolean = false;
  editedEmployee: any = {};

  public pageNumber: number | undefined = 1;
  public pageSize: number = 10;
  public Counter: number | undefined = 0;

  constructor(private employeeService: EmployeeService, private http: HttpClient) { }

  ngOnInit() {
    this.employees = this.getEmployees();
  }

  getEmployees() {
    this.employeeService.getAllEmployees(this.pageNumber, this.pageSize)
      .pipe(first())
      .subscribe(result => {
        console.log(result)
        this.employees = result;
        return this.employees;
      });
  }

  showEditEmployeeForm(employee: Employee) {
    if (!employee) {
      this.employeeForm = false;
      return;
    }
    this.editEmployeeForm = true;
    this.editedEmployee = employee;
    this.isNewEmployee = false;
    this.employeeForm = false;
  }

  showAddEmployeeForm() {
    // resets form if edited employee
    if (this.employees.length) {
      this.newEmployee = {};
    }
    this.employeeForm = true;
    this.isNewEmployee = true;
    this.editEmployeeForm = false;

  }

  saveEmployee(employee: Employee) {
    if (this.isNewEmployee) {

      this.http.post<any>(EmployeesController.Create, employee).subscribe({
        next: data => {
          if (!data.isPassed) {
            if (data.message)
              alert(data.message)
            else
              alert(data.errors[0])
            return;
          }
          else {
            this.employeeForm = false;
            this.getEmployees();
          }
        },
        error: error => {
          console.error('There was an error!', error);
          return throwError(error);
        }
      })
    }
  }

  updateEmployee(employee: Employee) {
    this.http.put<any>(EmployeesController.Update(employee.id), employee).subscribe({
      next: data => {
        if (!data.isPassed) {
          if (data.message)
            alert(data.message)
          else
            alert(data.errors[0])
          return;
        }
        else {
          this.editEmployeeForm = false;
          this.editedEmployee = {};
          this.getEmployees();
        }
      },
      error: error => {
        console.error('There was an error!', error);
        return throwError(error);
      }
    })
  }

  removeEmployee(employee: Employee) {
    this.http.delete<any>(EmployeesController.Remove(employee.id)).subscribe({
      next: data => {
        if (!data.isPassed) {
          if (data.message)
            alert(data.message)
          else
            alert(data.errors[0])
          return;
        }
        else {
          this.getEmployees();
        }
      },
      error: error => {
        console.error('There was an error!', error);
        return throwError(error);
      }
    })
  }

  cancelEdits() {
    this.editedEmployee = {};
    this.editEmployeeForm = false;
  }

  cancelNewEmployee() {
    this.newEmployee = {};
    this.employeeForm = false;
  }

}
