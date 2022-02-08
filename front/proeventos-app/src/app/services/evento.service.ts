import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, take } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable()

export class EventoService {
  baseURL = environment.api + "api/eventos";
  constructor(private http: HttpClient) { }

  getEvento() : Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL).pipe(take(1));
  }

  getEventoByTema(Tema: string) : Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/${Tema}/tema`).pipe(take(1));
  }

  getEventoById(Id: number) : Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${Id}`).pipe(take(1));
  }

  post(evento: Evento) : Observable<Evento> {
    return this.http.post<Evento>(this.baseURL, evento).pipe(take(1));
  }

  put(evento: Evento) : Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/${evento.id}`, evento).pipe(take(1));
  }

  deleteEvento(Id: number) : Observable<any> {
    return this.http.delete<any>(`${this.baseURL}/${Id}`).pipe(take(1));
  }

  postUpload(eventoId: number, file: File): Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData;
    formData.append('file', fileToUpload);

    return this.http.post<Evento>(`${this.baseURL}/upload-image/${eventoId}`, formData).pipe(take(1));
  }

}
