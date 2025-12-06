# 🧠 Portfolio AI Assistant Backend

Bu proje, kişisel portfolyo web sitem için geliştirilmiş, **Generative AI (Üretken Yapay Zeka)** destekli bir Backend servisidir. Ziyaretçilerin site sahibi hakkında sorduğu soruları (tecrübeler, projeler, teknik yetkinlikler vb.) doğal bir dille yanıtlayan akıllı bir köprü görevi görür.

Standart "Hakkımda" yazılarını okumak yerine, kullanıcıların **Göksel AI** ile sohbet ederek bilgi almasını sağlayan modern bir mimariye sahiptir.

---

## 🚀 Projenin Amacı ve Çözdüğü Sorun

Geleneksel portfolyo siteleri tek yönlüdür; ziyaretçi sadece okur. Bu proje ise bu deneyimi **çift yönlü ve etkileşimli** hale getirir.

* **Dinamik Etkileşim:** Kullanıcılar merak ettikleri spesifik soruları sorabilir.
* **Kişiselleştirilmiş Asistan:** Arka planda çalışan özel **System Prompt** mühendisliği sayesinde, bot rastgele cevaplar vermez; profesyonel bir "Kişisel Asistan" kimliğiyle, sadece benimle ilgili doğrulanmış bilgileri sunar.
* **7/24 Erişilebilirlik:** İnsan müdahalesi olmadan, potansiyel işverenlere veya ziyaretçilere anlık geri bildirim sağlar.

## 🏗️ Teknik Mimari ve Özellikler

Bu proje, modern Backend standartlarına uygun olarak **.NET Core** üzerinde inşa edilmiştir.

### 1. Yapay Zeka Entegrasyonu (Google Gemini)
Proje, Google'ın gelişmiş dil modeli **Gemini** ile entegre çalışır.
* Gelen kullanıcı mesajları API üzerinden modele iletilir.
* Model, önceden tanımlanmış "Persona" (Kimlik) kurallarına göre cevabı üretir ve frontend'e iletir.

### 2. Güvenlik ve CORS Politikaları
API halka açık bir sunucuda barındırıldığı için güvenlik ön planda tutulmuştur.
* **Origin Kısıtlaması:** Sadece `gokselkaradag.com.tr` domaininden gelen istekler kabul edilir. Diğer kaynaklardan gelen yetkisiz erişimler reddedilir.
* **Environment Yönetimi:** API anahtarları (API Keys) ve hassas veriler sunucu tarafında güvenli bir şekilde yönetilir.

### 3. RESTful Servis Yapısı
* Frontend (Web Sitesi) ve Backend (AI Servisi) tamamen birbirinden bağımsızdır.
* JSON formatında veri alışverişi yapan hafif ve hızlı bir yapıya sahiptir.

### 4. Deployment (Dağıtım)
* Uygulama **Self-Contained** modunda derlenmiş, sunucu bağımlılıkları ortadan kaldırılmıştır.
* Plesk panel üzerinde yönetilen Windows sunucuda canlı olarak çalışmaktadır.

## 🛠️ Teknoloji Yığını (Tech Stack)

| Alan | Teknoloji |
| :--- | :--- |
| **Framework** | .NET 10 Web API |
| **Language** | C# |
| **AI Provider** | Google Gemini API |
| **Architecture** | Layered Architecture (Servis Katmanı Mimarisi) |
| **Security** | CORS Policy Management |
| **Environment** | DotNetEnv |

## 🌐 Canlı Demo

Bu API'nin canlı çalışan halini ve entegre edildiği arayüzü aşağıdaki adresten deneyimleyebilirsiniz:

👉 **[gokselkaradag.com.tr](https://gokselkaradag.com.tr)**

*(Sayfanın sağ alt köşesindeki Chat butonuna tıklayarak asistan ile konuşabilirsiniz.)*

---
Developed with ❤️ by **Göksel Karadağ**
