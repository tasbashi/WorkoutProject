# Antrenör-Sporcu Takip Sistemi Proje Dokümantasyonu

## 1. Proje Özeti

Antrenör-Sporcu Takip Sistemi, spor salonları, fitness merkezleri ve kişisel antrenörler için geliştirilmiş kapsamlı bir performans takip ve yönetim platformudur. Sistem, antrenörler ve sporcular arasındaki iletişimi güçlendirirken, antrenman süreçlerini dijitalleştirerek verimliliği artırmayı hedefler.

## 2. Sistem Mimarisi

### 2.1 Teknoloji Stack'i

**Frontend:**
- React 18+ (TypeScript ile)
- Redux Toolkit (State Management)
- Material-UI veya Ant Design (UI Framework)
- React Router v6 (Routing)
- Axios (HTTP Client)
- Chart.js veya Recharts (Grafikler)
- React Hook Form + Yup (Form Validation)
- Socket.io-client (Gerçek zamanlı bildirimler)

**Backend:**
- ASP.NET Core 8.0 Web API
- Entity Framework Core 8 (ORM)
- SQL Server veya PostgreSQL (Database)
- Redis (Cache)
- SignalR (Real-time communication)
- JWT Authentication
- AutoMapper
- FluentValidation
- Serilog (Logging)
- Hangfire (Background Jobs)

**DevOps & Deployment:**
- Docker & Docker Compose
- Azure/AWS/Google Cloud
- CI/CD Pipeline (Azure DevOps/GitHub Actions)
- Nginx (Reverse Proxy)

## 3. Kullanıcı Rolleri ve Yetkilendirme

### 3.1 Roller

**Admin (Sistem Yöneticisi):**
- Tüm sistem üzerinde tam yetki
- Kullanıcı yönetimi
- Sistem ayarları ve konfigürasyonları
- Raporlama ve analitik

**Antrenör:**
- Sporcu ekleme/çıkarma
- Antrenman programı oluşturma
- Performans takibi ve raporlama
- Beslenme programı oluşturma
- Mesajlaşma ve bildirimler

**Sporcu:**
- Kendi verilerini görüntüleme
- Antrenman kayıtları ekleme
- İlerleme takibi
- Antrenörle iletişim

**Diyetisyen (Opsiyonel):**
- Beslenme programı oluşturma
- Kalori takibi
- Sporcu beslenme raporları

## 4. Detaylı Özellik Listesi

### 4.1 Kimlik Doğrulama ve Yetkilendirme Modülü

**Özellikler:**
- Çoklu rol desteği ile JWT tabanlı authentication
- Refresh token mekanizması
- İki faktörlü doğrulama (2FA)
- Şifre sıfırlama (e-posta ile)
- Hesap aktivasyonu
- Oturum yönetimi (çoklu cihaz desteği)
- OAuth2 entegrasyonu (Google, Facebook)
- Remember me özelliği
- Captcha koruması
- IP bazlı erişim kontrolü
- Hesap kilitleme mekanizması (başarısız deneme limiti)

### 4.2 Kullanıcı Profil Yönetimi

**Özellikler:**
- Detaylı profil bilgileri (ad, soyad, doğum tarihi, cinsiyet, boy, kilo)
- Profil fotoğrafı yükleme
- İletişim bilgileri
- Acil durum kişisi
- Sağlık geçmişi ve mevcut rahatsızlıklar
- Hedefler (kilo verme, kas kazanma, kondisyon vb.)
- Aktivite seviyesi
- Tercih edilen antrenman saatleri
- Vücut ölçüleri takibi (göğüs, bel, kalça, kol, bacak)
- Vücut yağ oranı takibi
- Sosyal medya hesapları bağlantısı

### 4.3 Antrenman Yönetimi

**Antrenman Programı Oluşturma:**
- Hazır şablonlar (başlangıç, orta, ileri seviye)
- Özelleştirilebilir program oluşturma
- Periyodizasyon desteği
- Split programlar (Push/Pull/Legs, Upper/Lower vb.)
- Supersetler ve dropsetler
- Dinlenme süreleri
- Progresif yükleme otomasyonu
- Video ve görsel açıklamalar
- Egzersiz kütüphanesi (1000+ egzersiz)
- Kas grubu bazlı filtreleme
- Ekipman bazlı filtreleme

**Antrenman Takibi:**
- Set, tekrar, ağırlık kayıtları
- Rest-pause, drop set, superset kayıtları
- Kardio aktiviteleri (süre, mesafe, kalori)
- RPE (Rate of Perceived Exertion) skoru
- Form videosu yükleme
- Antrenman notları
- Otomatik zamanlayıcı
- Ses bildirimleri
- Antrenman süresi takibi
- Yakılan kalori hesaplama

### 4.4 Performans Analizi ve Raporlama

**Grafikler ve İstatistikler:**
- Haftalık/Aylık/Yıllık performans grafikleri
- Kas grubu bazlı analiz
- 1RM (One Rep Max) hesaplama ve takibi
- Toplam kaldırılan ağırlık (volume)
- Antrenman sıklığı analizi
- İlerleme hızı göstergeleri
- Kişisel rekorlar (PR) takibi
- Vücut kompozisyonu değişimi
- Güç/Dayanıklılık oranları
- Isı haritası (hangi kas grupları ne sıklıkla çalışılıyor)

**Karşılaştırma Özellikleri:**
- Geçmiş dönemlerle karşılaştırma
- Diğer sporcularla anonim karşılaştırma
- Yaş/cinsiyet/kilo kategorisinde sıralama
- Hedef vs gerçekleşen karşılaştırması

### 4.5 Beslenme Takibi

**Özellikler:**
- Günlük kalori hedefi belirleme
- Makro besin takibi (Protein, Karbonhidrat, Yağ)
- Mikro besin takibi (vitaminler, mineraller)
- Su tüketimi takibi
- Yemek günlüğü
- Barcode ile ürün ekleme
- Öğün planlama
- Tarif oluşturma ve paylaşma
- Supplement takibi
- Cheat meal/refeed günleri
- Aralıklı oruç (IF) desteği
- Besin alerjileri ve kısıtlamaları

### 4.6 İletişim ve Sosyal Özellikler

**Mesajlaşma:**
- Antrenör-sporcu özel mesajlaşma
- Grup sohbetleri
- Sesli/görüntülü not bırakma
- Dosya paylaşımı
- Okundu bilgisi
- Mesaj arama

**Sosyal Özellikler:**
- Antrenman paylaşımları
- Başarı rozetleri ve ödüller
- Liderlik tabloları
- Challenges (meydan okumalar)
- Takım/grup oluşturma
- Motivasyon duvarı
- Başarı hikayeleri

### 4.7 Randevu ve Takvim Yönetimi

**Özellikler:**
- Antrenman randevusu oluşturma
- Takvim görünümü (gün/hafta/ay)
- Otomatik hatırlatmalar
- Randevu iptali/erteleme
- Müsaitlik takvimi
- Grup dersleri planlama
- Ödeme takibi entegrasyonu
- Check-in sistemi
- QR kod ile giriş

### 4.8 Ölçüm ve Değerlendirme

**Fiziksel Testler:**
- Vücut kompozisyonu analizi
- Esneklik testleri
- Kuvvet testleri
- Dayanıklılık testleri
- Denge ve koordinasyon testleri
- FMS (Functional Movement Screen)
- Postür analizi
- Fotoğraf karşılaştırmaları
- 3D vücut tarama entegrasyonu

### 4.9 Bildirim Sistemi

**Bildirim Türleri:**
- Push notifications
- E-posta bildirimleri
- SMS bildirimleri
- Uygulama içi bildirimler
- WhatsApp entegrasyonu

**Bildirim Senaryoları:**
- Antrenman hatırlatmaları
- Ölçüm hatırlatmaları
- Yeni program ataması
- Mesaj bildirimleri
- Başarı bildirimleri
- Randevu hatırlatmaları
- Motivasyon mesajları

### 4.10 Ödeme ve Abonelik Yönetimi

**Özellikler:**
- Farklı paket seçenekleri
- Online ödeme (kredi kartı, sanal pos)
- Otomatik faturalama
- Abonelik yenileme
- İndirim kuponları
- Referans sistemi
- Ödeme geçmişi
- Muhasebe raporları

### 4.11 Raporlama ve Analitik

**Antrenör Dashboard:**
- Aktif sporcu sayısı
- Aylık gelir özeti
- Sporcu devam oranları
- En popüler antrenman programları
- Sporcu memnuniyet anketleri
- Performans trendleri
- Kayıp sporcu analizi

**Admin Dashboard:**
- Sistem kullanım istatistikleri
- Kullanıcı büyüme oranları
- Gelir analizleri
- Hata ve performans logları
- A/B test sonuçları

### 4.12 Entegrasyonlar

**Wearable Cihazlar:**
- Apple Watch
- Fitbit
- Garmin
- Samsung Galaxy Watch
- Mi Band
- Polar

**Diğer Entegrasyonlar:**
- MyFitnessPal
- Strava
- Google Fit
- Apple Health
- Spotify (antrenman müzik listeleri)
- YouTube (egzersiz videoları)
- Zoom (online antrenman seansları)

## 5. Veritabanı Tasarımı

### 5.1 Temel Tablolar

**Users**
- Id (Guid)
- Username
- Email
- PasswordHash
- FirstName
- LastName
- DateOfBirth
- Gender
- PhoneNumber
- ProfilePicture
- CreatedAt
- UpdatedAt
- IsActive
- EmailConfirmed
- TwoFactorEnabled

**Roles**
- Id
- Name
- Description
- Permissions (JSON)

**UserRoles**
- UserId
- RoleId

**Athletes**
- Id
- UserId
- Height
- Weight
- ActivityLevel
- Goals
- MedicalHistory
- EmergencyContact
- TrainerId

**Trainers**
- Id
- UserId
- Specializations
- Certifications
- Experience
- Bio
- HourlyRate

**Exercises**
- Id
- Name
- Category
- MuscleGroups
- Equipment
- Instructions
- VideoUrl
- ImageUrl
- Difficulty

**WorkoutPrograms**
- Id
- TrainerId
- AthleteId
- Name
- Description
- StartDate
- EndDate
- Status
- ProgramType

**WorkoutSessions**
- Id
- ProgramId
- Date
- Duration
- CaloriesBurned
- Notes
- RPE
- Status

**WorkoutExercises**
- Id
- SessionId
- ExerciseId
- Sets
- Reps
- Weight
- RestTime
- Notes

**Measurements**
- Id
- AthleteId
- Date
- Weight
- BodyFatPercentage
- Chest
- Waist
- Hips
- Arms
- Legs

**NutritionPlans**
- Id
- AthleteId
- TrainerId
- Calories
- Protein
- Carbs
- Fat
- StartDate
- EndDate

**Messages**
- Id
- SenderId
- ReceiverId
- Content
- SentAt
- ReadAt
- IsDeleted

**Appointments**
- Id
- TrainerId
- AthleteId
- DateTime
- Duration
- Status
- Notes

**Payments**
- Id
- AthleteId
- Amount
- PaymentDate
- PaymentMethod
- Status
- InvoiceNumber

## 6. API Endpoint Tasarımı

### 6.1 Authentication Endpoints

```
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh
POST /api/auth/logout
POST /api/auth/forgot-password
POST /api/auth/reset-password
POST /api/auth/verify-email
POST /api/auth/enable-2fa
POST /api/auth/verify-2fa
```

### 6.2 User Management Endpoints

```
GET /api/users/profile
PUT /api/users/profile
POST /api/users/upload-avatar
DELETE /api/users/account
GET /api/users/notifications
PUT /api/users/notifications/read
```

### 6.3 Trainer Endpoints

```
GET /api/trainers/athletes
GET /api/trainers/athletes/{id}
POST /api/trainers/athletes
DELETE /api/trainers/athletes/{id}
GET /api/trainers/dashboard
GET /api/trainers/schedule
POST /api/trainers/availability
```

### 6.4 Athlete Endpoints

```
GET /api/athletes/profile
PUT /api/athletes/measurements
GET /api/athletes/progress
GET /api/athletes/workouts
GET /api/athletes/nutrition
```

### 6.5 Workout Endpoints

```
GET /api/workouts/programs
GET /api/workouts/programs/{id}
POST /api/workouts/programs
PUT /api/workouts/programs/{id}
DELETE /api/workouts/programs/{id}
POST /api/workouts/sessions
PUT /api/workouts/sessions/{id}
GET /api/workouts/exercises
GET /api/workouts/templates
```

### 6.6 Analytics Endpoints

```
GET /api/analytics/performance
GET /api/analytics/progress
GET /api/analytics/comparison
GET /api/analytics/personal-records
GET /api/analytics/muscle-distribution
```

## 7. Güvenlik Gereksinimleri

### 7.1 Kimlik Doğrulama ve Yetkilendirme
- JWT token ile authentication
- Refresh token rotasyonu
- Role-based access control (RBAC)
- API rate limiting
- IP whitelisting (admin panel için)

### 7.2 Veri Güvenliği
- HTTPS zorunluluğu
- Hassas verilerin şifrelenmesi
- SQL injection koruması
- XSS koruması
- CSRF token kullanımı
- Input validation
- Output encoding

### 7.3 GDPR/KVKK Uyumluluğu
- Kullanıcı verilerinin anonimleştirilmesi
- Veri silme hakkı
- Veri taşınabilirliği
- Açık rıza mekanizması
- Veri işleme sözleşmeleri

## 8. Performans Gereksinimleri

### 8.1 Yanıt Süreleri
- API yanıt süresi: < 200ms (ortalama)
- Sayfa yükleme süresi: < 2 saniye
- Database query süresi: < 100ms

### 8.2 Ölçeklenebilirlik
- Horizontal scaling desteği
- Load balancing
- Database sharding
- CDN kullanımı
- Redis cache implementasyonu

### 8.3 Kullanılabilirlik
- %99.9 uptime garantisi
- Otomatik backup (günlük)
- Disaster recovery planı
- Health check endpoints
- Monitoring ve alerting

## 9. Geliştirme Süreci - Developer Roadmap

### Faz 1: Proje Kurulumu ve Temel Altyapı (2 Hafta)

**Backend Görevleri:**
1. ASP.NET Core 8 Web API projesi oluşturma
2. Proje yapısının kurulması (Clean Architecture/Onion Architecture)
3. Entity Framework Core konfigürasyonu
4. Database migration stratejisi belirleme
5. Logging altyapısının kurulması (Serilog)
6. Exception handling middleware
7. API versioning implementasyonu
8. Swagger/OpenAPI dokümantasyonu
9. Docker containerization
10. Unit test ve integration test altyapısı

**Frontend Görevleri:**
1. React + TypeScript proje kurulumu
2. Folder structure oluşturma
3. Redux Toolkit konfigürasyonu
4. Routing yapısının kurulması
5. UI framework entegrasyonu
6. Axios interceptor'ları
7. Environment konfigürasyonları
8. ESLint ve Prettier ayarları
9. Git hooks (Husky) kurulumu
10. Component library başlangıcı

### Faz 2: Kimlik Doğrulama ve Kullanıcı Yönetimi (2 Hafta)

**Backend Görevleri:**
1. Identity framework entegrasyonu
2. JWT token implementasyonu
3. Refresh token mekanizması
4. User ve Role entity'leri
5. Registration endpoint
6. Login/Logout endpoints
7. Password reset flow
8. Email service entegrasyonu
9. 2FA implementasyonu
10. Authorization policies

**Frontend Görevleri:**
1. Login sayfası
2. Register sayfası
3. Forgot password flow
4. Auth context/hooks
5. Protected routes
6. Token management
7. Auto refresh token logic
8. Logout functionality
9. User profile component
10. Role-based UI rendering

### Faz 3: Temel CRUD Operasyonları (3 Hafta)

**Backend Görevleri:**
1. Exercise entity ve repository
2. WorkoutProgram entity ve service
3. Athlete entity ve ilişkiler
4. Trainer entity ve ilişkiler
5. Generic repository pattern
6. Unit of Work pattern
7. AutoMapper konfigürasyonları
8. Validation rules (FluentValidation)
9. Business logic layer
10. API endpoints (CRUD)

**Frontend Görevleri:**
1. Exercise library UI
2. Program creation wizard
3. Athlete dashboard
4. Trainer dashboard
5. Form components
6. Data tables
7. Search ve filter components
8. Pagination
9. Loading states
10. Error handling

### Faz 4: Antrenman Takip Modülü (3 Hafta)

**Backend Görevleri:**
1. WorkoutSession entity
2. WorkoutExercise entity
3. Set/Rep tracking logic
4. Performance calculation services
5. Progress tracking algorithms
6. Volume calculation
7. 1RM calculation service
8. Rest timer logic
9. Workout template system
10. Bulk data operations

**Frontend Görevleri:**
1. Workout logging interface
2. Exercise selection modal
3. Set/Rep/Weight inputs
4. Rest timer component
5. Workout history view
6. Calendar integration
7. Quick log features
8. Superset/Dropset UI
9. Video upload for form check
10. Workout notes and feedback

### Faz 5: Performans Analizi ve Raporlama (2 Hafta)

**Backend Görevleri:**
1. Analytics service layer
2. Performance metrics calculation
3. Progress tracking queries
4. Aggregate data endpoints
5. Report generation service
6. Export functionality (PDF/Excel)
7. Caching strategy
8. Query optimization
9. Background job for reports
10. Data warehouse considerations

**Frontend Görevleri:**
1. Chart components (Chart.js/Recharts)
2. Performance dashboard
3. Progress graphs
4. Comparison views
5. Personal records display
6. Heat maps
7. Statistical summaries
8. Export buttons
9. Date range selectors
10. Custom report builder

### Faz 6: Beslenme Takibi (2 Hafta)

**Backend Görevleri:**
1. Nutrition entities
2. Food database integration
3. Calorie calculation service
4. Macro tracking logic
5. Meal plan generator
6. Recipe management
7. Barcode API integration
8. Nutrition goals service
9. Water intake tracking
10. Supplement tracking

**Frontend Görevleri:**
1. Food diary interface
2. Meal planning calendar
3. Macro calculator
4. Food search component
5. Barcode scanner (mobile)
6. Recipe creator
7. Nutrition dashboard
8. Goal setting UI
9. Progress charts
10. Meal suggestions

### Faz 7: İletişim ve Bildirim Sistemi (2 Hafta)

**Backend Görevleri:**
1. SignalR hub implementation
2. Message entity ve service
3. Notification service
4. Email templates
5. SMS integration
6. Push notification service
7. Real-time message delivery
8. Message history
9. Group chat functionality
10. File sharing in messages

**Frontend Görevleri:**
1. Chat interface
2. Notification center
3. Real-time updates
4. Message threads
5. Typing indicators
6. Read receipts
7. File attachments
8. Emoji support
9. Push notification handling
10. Notification preferences

### Faz 8: Randevu ve Takvim (1 Hafta)

**Backend Görevleri:**
1. Appointment entity
2. Availability management
3. Scheduling algorithm
4. Reminder service
5. Calendar sync (Google/Outlook)
6. Recurring appointments
7. Cancellation logic
8. Waitlist functionality
9. Time zone handling
10. Conflict detection

**Frontend Görevleri:**
1. Calendar component
2. Appointment booking UI
3. Availability setter
4. Schedule view
5. Reminder settings
6. Drag-drop rescheduling
7. Mobile calendar view
8. Quick booking
9. Appointment details modal
10. Cancellation flow

### Faz 9: Ödeme Sistemi (2 Hafta)

**Backend Görevleri:**
1. Payment gateway integration (Stripe/PayPal)
2. Subscription management
3. Invoice generation
4. Payment history
5. Refund processing
6. Coupon system
7. Trial period logic
8. Payment webhooks
9. Financial reporting
10. Tax calculations

**Frontend Görevleri:**
1. Payment form
2. Subscription plans UI
3. Payment history page
4. Invoice viewer
5. Payment method management
6. Coupon input
7. Upgrade/downgrade flow
8. Billing dashboard
9. Payment confirmation
10. Error handling

### Faz 10: Mobil Optimizasyon ve PWA (1 Hafta)

**Görevler:**
1. Responsive design review
2. Touch gestures
3. Service worker implementation
4. Offline functionality
5. App manifest
6. Push notifications (mobile)
7. Camera integration
8. GPS features
9. Performance optimization
10. PWA deployment

### Faz 11: Entegrasyonlar (2 Hafta)

**Görevler:**
1. Wearable device APIs
2. MyFitnessPal integration
3. Strava API
4. Google Fit/Apple Health
5. Social media sharing
6. Video platform integration
7. Payment gateway testing
8. Email service provider
9. SMS gateway
10. Cloud storage integration

### Faz 12: Test, Optimizasyon ve Deployment (2 Hafta)

**Görevler:**
1. Unit test coverage (%80+)
2. Integration tests
3. E2E testing (Cypress)
4. Performance testing
5. Security testing
6. Load testing
7. User acceptance testing
8. Bug fixes
9. Performance optimization
10. Production deployment

### Faz 13: Post-Launch ve Bakım

**Görevler:**
1. Monitoring setup
2. Error tracking (Sentry)
3. Analytics implementation
4. A/B testing framework
5. User feedback collection
6. Performance monitoring
7. Security updates
8. Feature iterations
9. Documentation updates
10. User training materials

## 10. Test Stratejisi

### 10.1 Test Türleri
- Unit Tests (%80+ coverage)
- Integration Tests
- API Tests
- UI Tests (Cypress/Selenium)
- Performance Tests (JMeter)
- Security Tests
- Usability Tests
- Regression Tests

### 10.2 Test Ortamları
- Development
- Testing/QA
- Staging
- Production

## 11. Dokümantasyon

### 11.1 Teknik Dokümantasyon
- API dokümantasyonu (Swagger)
- Database şeması
- Deployment guide
- Architecture diagrams
- Code standards
- Git workflow

### 11.2 Kullanıcı Dokümantasyonu
- User manual
- Video tutorials
- FAQ
- Troubleshooting guide
- Feature guides

## 12. Proje Yönetimi

### 12.1 Metodoloji
- Agile/Scrum
- 2 haftalık sprintler
- Daily standup
- Sprint planning
- Sprint retrospective

### 12.2 Araçlar
- Jira/Azure DevOps (Project Management)
- Git (Version Control)
- Slack/Teams (Communication)
- Figma (Design)
- Postman (API Testing)

## 13. Risk Analizi ve Yönetimi

### 13.1 Teknik Riskler
- Ölçeklenebilirlik sorunları
- Güvenlik açıkları
- Third-party servis kesintileri
- Data loss
- Performance degradation

### 13.2 İş Riskleri
- Bütçe aşımı
- Zaman aşımı
- Scope creep
- Kullanıcı adaptasyonu
- Rekabet

### 13.3 Risk Azaltma Stratejileri
- Regular backups
- Load testing
- Security audits
- Progressive rollout
- Feature flags
- A/B testing
- User feedback loops

## 14. Başarı Metrikleri

### 14.1 Teknik Metrikler
- Page load time < 2s
- API response time < 200ms
- Uptime > 99.9%
- Bug count < 5 per sprint
- Code coverage > 80%

### 14.2 İş Metrikleri
- User retention rate
- Monthly active users
- Customer satisfaction score
- Revenue per user
- Churn rate
- Feature adoption rate

## 15. Gelecek Geliştirmeler

### 15.1 Kısa Vadeli (3-6 ay)
- AI-powered workout recommendations
- Voice commands
- AR form correction
- Social features enhancement
- Advanced analytics

### 15.2 Uzun Vadeli (6-12 ay)
- Multi-language support
- Franchise management
- Virtual reality workouts
- AI nutrition coach
- Predictive injury prevention
- Competition platform
- Marketplace for programs

Bu dokümantasyon, projenin tüm yönlerini kapsayan detaylı bir yol haritası sunmaktadır. Her fazda belirtilen görevler sırasıyla tamamlanmalı ve her aşamada code review, testing ve documentation süreçleri takip edilmelidir. Projenin başarısı için düzenli sprint retrospektifleri yapılmalı ve kullanıcı geri bildirimleri sürekli olarak değerlendirilmelidir.~~~~~~~~