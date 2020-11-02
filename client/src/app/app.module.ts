import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './router/app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthInterceptor } from './auth/auth.interceptor';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { environment } from 'src/environments/environment';
import { BaseUrlInterceptor } from './interceptors/base-url.interceptor';
import { SignUpComponent } from './sign-up/sign-up.component';
import { HttpErrorInterceptor } from './interceptors/http-error.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { HeaderComponent } from './header/header.component';

const MATERIAL_COMPONENTS = [MatButtonModule, MatFormFieldModule, MatInputModule, MatIconModule, MatToolbarModule];

@NgModule({
  declarations: [AppComponent, LoginComponent, SignUpComponent, HeaderComponent],
  imports: [
    ...MATERIAL_COMPONENTS,
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    ToastrModule.forRoot(),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: BaseUrlInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true,
    },
    {
      provide: 'BASE_API_URL',
      useValue: environment.apiUrl,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
