import { Component, OnInit } from '@angular/core';
import { AbstractControl, AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';


@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})

export class PerfilComponent implements OnInit {

  form : FormGroup = this.FormBuilder.group({});

  constructor(public FormBuilder: FormBuilder) { }

  get f(): any{
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
  }

  private validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmeSenha')
    }

    this.form = this.FormBuilder.group({
      titulo:['', [Validators.required]],
      primeiroNome:['', [Validators.required]],
      ultimoNome:['', [Validators.required]],
      email:['', [Validators.required, Validators.email]],
      telefone:['', [Validators.required]],
      funcao:['', [Validators.required]],
      descricao:['', [Validators.required, Validators.maxLength(100)]],
      senha:['', [Validators.required, Validators.minLength(8)]],
      confirmeSenha:['', [Validators.required, Validators.minLength(8)]],
    }, formOptions)
  }

  public resetForm(event: any): void {
    event.preventDefault();
    this.form.reset();
  }

  public onSubmit(): void {
    if (this.form.invalid){
      return;
    }
  }

}
