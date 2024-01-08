import { Component, OnInit } from '@angular/core';
import { NoteService } from '../services/note.service';
import { Note } from '../models/note';
import { AuthStateService } from '../services/auth-state.service';

@Component({
    selector: 'app-notes',
    templateUrl: './notes.component.html',
    styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {
    notes: Note[] = [];
    editing = false;
    editNoteId: number | null = null;
    editNoteTitle = '';
    editNoteContent = '';
    newNoteTitle = '';
    selectedNoteId: number | null = null;
    newNoteContent: string = '';

  constructor(private noteService: NoteService, private authStateService: AuthStateService) {}

  ngOnInit() {
    this.getNotes();
  }

  get isAuthenticated(): boolean {
    return this.authStateService.getAuthState();
  }

  toggleNoteDetails(noteId: number) {
    if (this.selectedNoteId === noteId) {
      this.selectedNoteId = null;
    } else {
      this.selectedNoteId = noteId;
    }
  }

  getNotes() {
    this.noteService.getNotes().subscribe(notes => {
      this.notes = notes;
    });
  }

  addNote() {
    const newNote: Note = {
      title: this.newNoteTitle,
      content: this.newNoteContent,
      id: 0 
    };
  
    this.noteService.addNote(newNote).subscribe(() => {
      this.newNoteTitle = '';
      this.newNoteContent = '';
      this.getNotes();
    });
  }
  
  editNote(id: number) {
    this.editing = true;
    this.editNoteId = id;
    const note = this.notes.find(n => n.id === id);
    if (note) {
      this.editNoteTitle = note.title;
      this.editNoteContent = note.content;
    }
  }

  saveEdit() {
    if (this.editNoteId !== null) {
        const updatedNote: Note = {
          id: this.editNoteId,
          title: this.editNoteTitle,
          content: this.editNoteContent
        };
    
        this.noteService.updateNote(this.editNoteId, updatedNote).subscribe(
            () => {
              this.editing = false;
              this.editNoteId = null;
              this.editNoteTitle = '';
              this.editNoteContent = '';
              this.getNotes();
            },
            error => {
              console.error('Error updating note:', error);

              if (error.error) {
                console.error('Response body:', error.error);}

            }
          );
    }
  }

  cancelEdit() {
    this.editing = false;
    this.editNoteId = null;
    this.editNoteTitle = '';
    this.editNoteContent = '';
  }

  deleteNote(id: number) {
    if (id !== undefined) {
        this.noteService.deleteNote(id).subscribe(() => {
          this.getNotes();
        });
      }
  }
}