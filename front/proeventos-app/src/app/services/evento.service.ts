import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable()

export class EventoService {
  baseURL = "http://localhost:5000/api/eventos";
  constructor(private http: HttpClient) { }

  getEvento() : Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL);
  }

  getEventoByTema(Tema: string) : Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/${Tema}/tema`);
  }

  getEventoById(Id: number) : Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${Id}`);
  }

}
