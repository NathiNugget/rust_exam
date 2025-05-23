use actix_web::http::StatusCode;
use actix_web::web::Data;
use actix_web::{get, post, web, App, HttpResponse, HttpServer, Responder};
use serde::Deserialize;
use std::sync::Mutex;

#[derive(Debug, Deserialize)]
struct MessageDTO {
    msg: String,
}

#[actix_web::main]
async fn main() -> std::io::Result<()> {
    let mutex :Mutex<Vec<String>> = Mutex::new(Vec::with_capacity(100000));


    let data :Data<Mutex<Vec<String>>> = Data::new(mutex);
    HttpServer::new( move|| {
       App::new()
           .app_data(data.clone())
           .service(get_notes)
           .service(add_note)

    }).bind("0.0.0.0:8080")?.run().await


}

#[get("/notes")]
async fn get_notes(notes: Data<Mutex<Vec<String>>>) -> impl Responder {
    let lock_attempt = notes.lock();
    match lock_attempt {
        Ok(val) => {
            HttpResponse::Ok().body(format!("{:?}", val))
        }
        Err(e) => { HttpResponse::InternalServerError().body(format!("Noget gik galt på serveren {:?}", e)) }
    }
}

#[post("/notes")]
async fn add_note(notes: Data<Mutex<Vec<String>>>, note: web::Json<MessageDTO>) -> impl Responder {
    let lock_attempt = notes.lock();
    match lock_attempt {
        Ok(mut notes_vec) => {
            notes_vec.push(note.msg.clone());
            HttpResponse::new(StatusCode::NO_CONTENT)
        }
        Err(e) => {
            HttpResponse::InternalServerError().body(format!("Noget gik galt på serveren {:?}", e))
        }
    }







}