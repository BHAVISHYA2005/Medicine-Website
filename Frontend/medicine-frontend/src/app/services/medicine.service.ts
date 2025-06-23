import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Medicine, MedicineSearchRequest } from '../models/medicine.model';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root'
})
export class MedicineService {
  private apiUrl = `${environment.apiUrl}/medicines`;

  constructor(private http: HttpClient) { }

  searchMedicines(searchRequest: MedicineSearchRequest): Observable<Medicine[]> {
    return this.http.post<Medicine[]>(`${this.apiUrl}/search`, searchRequest);
  }

  getMedicine(id: number): Observable<Medicine> {
    return this.http.get<Medicine>(`${this.apiUrl}/${id}`);
  }

  getMedicinesByPharmacy(pharmacyId: number): Observable<Medicine[]> {
    return this.http.get<Medicine[]>(`${this.apiUrl}/pharmacy/${pharmacyId}`);
  }

  getAlternatives(medicineId: number): Observable<Medicine[]> {
    return this.http.get<Medicine[]>(`${this.apiUrl}/${medicineId}/alternatives`);
  }

  checkAvailability(medicineId: number, location: string): Observable<boolean> {
    return this.http.get<boolean>(`${this.apiUrl}/${medicineId}/availability`, {
      params: { location }
    });
  }
}
