import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Lote } from '../models/Lote';


@Injectable()
export class LoteService {

  baseURL = "http://localhost:5000/api/lotes";
  constructor(private http: HttpClient) { }

  getLotesByEventoId(eventoId: number) : Observable<Lote[]> {
    return this.http.get<Lote[]>(`${this.baseURL}/${eventoId}`).pipe(take(1));
  }

  save(eventoId: number, lotes: Lote[]) : Observable<Lote> {
    return this.http.put<Lote>(`${this.baseURL}/${eventoId}`, lotes).pipe(take(1));
  }

  deleteLote(eventoId: number, loteId: number) : Observable<any> {
    return this.http.delete<any>(`${this.baseURL}/${eventoId}/${loteId}`).pipe(take(1));
  }

}
