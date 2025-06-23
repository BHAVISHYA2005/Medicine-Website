import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../environment/environment';

export interface Pharmacy {
  id: string;
  name: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  phone: string;
  email: string;
  latitude: number;
  longitude: number;
  rating: number;
  isActive: boolean;
  openingHours: string;
  closingHours: string;
  deliveryAvailable: boolean;
  pharmacyMedicines: PharmacyMedicine[];
}

export interface PharmacyMedicine {
  id: string;
  pharmacyId: string;
  medicineId: string;
  price: number;
  stockQuantity: number;
  isAvailable: boolean;
  discountPercentage?: number;
  medicine: any;
}

export interface PharmacySearchParams {
  location?: string;
  latitude?: number;
  longitude?: number;
  radius?: number;
  name?: string;
  deliveryAvailable?: boolean;
  isOpen?: boolean;
  page?: number;
  pageSize?: number;
}

@Injectable({
  providedIn: 'root'
})
export class PharmacyService {

  constructor(private http: HttpClient) { }

  searchPharmacies(params: PharmacySearchParams): Observable<any> {
    let httpParams = new HttpParams();
    
    if (params.location) httpParams = httpParams.set('location', params.location);
    if (params.latitude) httpParams = httpParams.set('latitude', params.latitude.toString());
    if (params.longitude) httpParams = httpParams.set('longitude', params.longitude.toString());
    if (params.radius) httpParams = httpParams.set('radius', params.radius.toString());
    if (params.name) httpParams = httpParams.set('name', params.name);
    if (params.deliveryAvailable !== undefined) httpParams = httpParams.set('deliveryAvailable', params.deliveryAvailable.toString());
    if (params.isOpen !== undefined) httpParams = httpParams.set('isOpen', params.isOpen.toString());
    if (params.page) httpParams = httpParams.set('page', params.page.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());

    return this.http.get<any>(`${environment.apiUrl}/pharmacies/search`, { params: httpParams });
  }

  getPharmacyById(id: string): Observable<Pharmacy> {
    return this.http.get<Pharmacy>(`${environment.apiUrl}/pharmacies/${id}`);
  }

  getPharmacyMedicines(pharmacyId: string, medicineId?: string): Observable<PharmacyMedicine[]> {
    let params = new HttpParams();
    if (medicineId) {
      params = params.set('medicineId', medicineId);
    }
    
    return this.http.get<PharmacyMedicine[]>(`${environment.apiUrl}/pharmacies/${pharmacyId}/medicines`, { params });
  }

  getNearbyPharmacies(latitude: number, longitude: number, radius: number = 10): Observable<Pharmacy[]> {
    const params = new HttpParams()
      .set('latitude', latitude.toString())
      .set('longitude', longitude.toString())
      .set('radius', radius.toString());

    return this.http.get<Pharmacy[]>(`${environment.apiUrl}/pharmacies/nearby`, { params });
  }

  getPharmacyRating(pharmacyId: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/pharmacies/${pharmacyId}/rating`);
  }

  ratePharmacy(pharmacyId: string, rating: number, comment?: string): Observable<any> {
    const body = { rating, comment };
    return this.http.post(`${environment.apiUrl}/pharmacies/${pharmacyId}/rate`, body);
  }

  checkPharmacyAvailability(pharmacyId: string): Observable<any> {
    return this.http.get(`${environment.apiUrl}/pharmacies/${pharmacyId}/availability`);
  }

  getCurrentLocation(): Promise<GeolocationPosition> {
    return new Promise((resolve, reject) => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(resolve, reject);
      } else {
        reject(new Error('Geolocation is not supported by this browser.'));
      }
    });
  }
}
