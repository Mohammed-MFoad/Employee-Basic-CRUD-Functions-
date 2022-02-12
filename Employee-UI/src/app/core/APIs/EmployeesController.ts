
import { environment } from 'src/environments/environment';

export const EmployeesController = {
  Search: environment.baseURL + `/api/employees`,
  Dropdown: environment.baseURL + `/api/employees/dropdown`,
  Create: environment.baseURL + `/api/employees`,
  Update: (id: number) => environment.baseURL + `/api/employees/${id}`,
  Remove: (id: number) => environment.baseURL + `/api/employees/${id}`,
  Activate: (id: number) => environment.baseURL + `/api/employees/${id}/activate`,
  Deactivate: (id: number) => environment.baseURL + `/api/employees/${id}/deactivate`,
}