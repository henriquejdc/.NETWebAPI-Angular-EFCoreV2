import { Component, OnInit } from '@angular/core';
import { ValidationErrors } from '@angular/forms';
import {
  FormGroup,
  FormBuilder,
  FormArray,
  FormControl,
  Validators
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})

export class EventoDetalheComponent implements OnInit {

  evento = {} as Evento;
  form : FormGroup = this.FormBuilder.group({});
  estadoSalvar = 'post';

  get f(): any{
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  constructor(private FormBuilder: FormBuilder,
              private localeService: BsLocaleService,
              private router: ActivatedRoute,
              private eventoServico: EventoService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService)
  {
    this.localeService.use('pt-br');
  }

  public carregarEvento() : void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');


    if (eventoIdParam !== null)
    {
      this.estadoSalvar = 'put';
      this.spinner.show();
      this.eventoServico.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
        },
        error: (error: any) => {
          this.spinner.hide(),
          this.toastr.error('Erro ao carrgar evento!', 'Erro!'),
          console.error(error)
        },
        complete: () => {this.spinner.hide()},
      });
    }
  }

  ngOnInit(): void {
    this.carregarEvento();
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

  public cssValidator(camppoForm: FormControl): any {
    return {'is-invalid': camppoForm.errors && camppoForm.touched}
  }

  public salvarAlteracao() : void {
    this.spinner.show();
    if (this.form.valid){

      if (this.estadoSalvar === 'post'){
        this.evento = {... this.form.value}
      } else {
        this.evento = {id: this.evento.id, ... this.form.value}
      }

      this.eventoServico[this.estadoSalvar](this.evento).subscribe(
        () => {this.toastr.success('Evento salvo com sucesso.', 'Salvo!')},
        (error: any) => {
          console.log(error);
          this.toastr.error('Error ao salvar.', 'Erro!')
        }
      ).add(() => this.spinner.hide());
    }
  }

}
