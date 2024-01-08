import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Note } from '../models/note';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  private apiUrl = 'Notes';

  constructor(private http: HttpClient) { }

  getNotes(): Observable<Note[]> {
    return this.http.get<Note[]>(`${environment.apiUrl}/${this.apiUrl}`);
  }

  getNoteById(id: number): Observable<Note> {
    return this.http.get<Note>(`${environment.apiUrl}/${this.apiUrl}/${id}`);
  }

  addNote(note: Note): Observable<Note> {
    return this.http.post<Note>(`${environment.apiUrl}/${this.apiUrl}`, note);
  }

  updateNote(id: number, note: Note): Observable<any> {
    return this.http.put(`${environment.apiUrl}/${this.apiUrl}/${id}`, note);
  }

  deleteNote(id: number): Observable<any> {
    return this.http.delete(`${environment.apiUrl}/${this.apiUrl}/${id}`);
  }
}