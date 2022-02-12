import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmployeesController } from './../APIs/EmployeesController';
import { Employee } from '../models/Employee';


@Injectable({ providedIn: 'root' })
export class EmployeeService {
  constructor(private http: HttpClient) { }

  getAllEmployees(pageNumber: number | undefined, pageSize: number | undefined) {
    return this.http
      .get<Employee>(EmployeesController.Search + "?PageIndex=" + pageNumber + "&PageSize=" + pageSize);
  }
}