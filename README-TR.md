# İş Takip Sistemi Projesi

## Proje Amacı

**İş Takip Sistemi Projesi**, iş yerlerinde görevlerin ve çalışanların performansının etkin bir şekilde yönetilmesini sağlayan bir platformdur. Bu sistemde, farklı yetkilere sahip üç tür kullanıcı bulunmaktadır:

1. **Yönetici**: 
   - Çalışanlara iş atayabilir ve bu işlerin takibini yapabilir.
   - Çalışanların izin taleplerini onaylayabilir veya reddedebilir.
   - Duyurular oluşturabilir.

2. **Çalışan**: 
   - Kendilerine atanan işleri kabul edebilir veya reddedebilir.
   - Atanan işleri tamamlayabilir ve işler hakkında yorum yapabilir.
   - Kendi işlerini takip edebilir.
   - Diğer personellere sistem üzerinden e-posta gönderebilir.

3. **Admin**: 
   - Sistemin tüm verilerine erişebilir ve bu verileri değiştirebilir.
   - Personel ekleme/silme, birim ekleme/silme, log işlemleri, yemek menüsü oluşturma gibi birçok işlemi yapabilir.
   - Yöneticiler gibi duyuru oluşturabilir.

**Not:** Sistemde yalnızca bir Admin bulunabilir ve her birimin yalnızca bir yöneticisi olabilir.

## Özellikler

- **Görev Yönetimi**: Yöneticiler tarafından atanan görevler, çalışanlar tarafından kabul edilebilir ve tamamlanabilir.
- **İzin Yönetimi**: Çalışanların izin talepleri yöneticiler tarafından onaylanabilir veya reddedilebilir.
- **Duyurular**: Yöneticiler ve adminler tarafından duyurular oluşturulabilir.
- **Mail Sistemi**: Personeller arasında e-posta gönderimi yapılabilir.
- **Birim ve Personel Yönetimi**: Admin, birim ve personel işlemlerini yönetebilir.
- **Yemek Menüsü Oluşturma**: Admin, günlük yemek menülerini oluşturabilir.

## Kullanılan Teknolojiler

- **Backend**: .NET MVC, MSSQL, SQL
- **Frontend**: Bootstrap, JavaScript, HTML, CSS, Razor
- **Grafikler**: chart.js

## Kurulum

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

1. **Depoyu klonlayın:**

   ```bash
   git clone https://github.com/lionpeace/Employee-Tracking-System-Project.git
   cd Employee-Tracking-System-Project

2. **Veritabanını Kurun:**

  - MSSQL Server üzerinde gerekli tabloları oluşturun
  - **appsettings.json** dosyasında veritabanı bağlantı ayarlarını yapın

3. **Gerekli Bağımlılıkları Yükleyin**

    ```bash
   dotnet restore

4. **Projeyi Çalıştırın**

    ```bash
   dotnet run

## Kullanım

  - **Yönetici Paneli**: Yöneticiler ve adminler, görev atama ve izin yönetimi işlemlerini buradan yapabilir.
  - **Çalışan Paneli**: Çalışanlar, kendilerine atanan görevleri burada görebilir ve tamamlayabilir.
  - **Admin Paneli**: Sistem yönetimi işlemleri buradan gerçekleştirilir.

## Ekran Görüntüleri

   <div align="center">
   ### Giriş Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/1.png" alt="Manager Panel"/>

   ### Yönetici Ana Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/2.png" alt="Manager Panel"/>

   ### İş Atama İşlemleri Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/3.png" alt="Manager Panel"/>

   ### İş Takip Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/4.png" alt="Manager Panel"/>

   ### İzinleri Yönet Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/5.png" alt="Manager Panel"/>

   ### Mail Gelen Kutusu Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/6.png" alt="Manager Panel"/>

   ### Mail Gönder Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/7.png" alt="Manager Panel"/>

   ### Duyuru Okuma Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/8.png" alt="Manager Panel"/>

   ### Yemek Tablosu Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/9.png" alt="Manager Panel"/>

   ### Log Tablosu Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/10.png" alt="Manager Panel"/>

   ### Birim Tablosu Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/11.png" alt="Manager Panel"/>

   ### Personel Tablosu Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/12.png" alt="Manager Panel"/>

   ### Çalışan Ana Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/13.png" alt="Manager Panel"/>

   ### İş Tamamlama Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/14.png" alt="Manager Panel"/>

   ### İş Takip Sayfası (Çalışan)
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/15.png" alt="Manager Panel"/>

   ### İzin Talep Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/16.png" alt="Manager Panel"/>

   ### İzinlerim Sayfası
   <img src="https://bariscanaslan.com/Github/Employee-Tracking-System-Project/17.png" alt="Manager Panel"/>
</div>

   
