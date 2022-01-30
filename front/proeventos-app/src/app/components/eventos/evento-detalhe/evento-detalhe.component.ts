import { Component, OnInit } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import {
  FormGroup,
  FormBuilder,
  FormArray,
  FormControl,
  Validators
} from '@angular/forms';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})

export class EventoDetalheComponent implements OnInit {

  form : FormGroup = this.FormBuilder.group({});

  get f(): any{
    return this.form.controls;
  }

  constructor(private FormBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void{
    this.form = this.FormBuilder.group({
      tema: ['', [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [
          Validators.required,
          Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [
          Validators.required,
          Validators.email
        ]],
      imagemURL: ['', Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

}
