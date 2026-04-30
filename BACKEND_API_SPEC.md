# EduTrack — Backend API Spesifikasyonu

> Bu doküman, EduTrack mobil uygulamasının tüm ekranlarındaki mock veriler analiz edilerek
> hazırlanmıştır. Clean Architecture prensipleri gözetilerek domain boundary'lere göre
> gruplanmıştır. Her endpoint için HTTP method, URL, request body, başarılı response ve
> hata response'ları örnek JSON ile verilmiştir.

---

## Genel Kurallar

| Kural | Değer |
|---|---|
| Base URL | `https://api.edutrack.com/v1` |
| İçerik tipi | `Content-Type: application/json` |
| Kimlik doğrulama | `Authorization: Bearer <JWT_ACCESS_TOKEN>` |
| Timezone | UTC (ISO 8601 formatı) |
| Sayfalama | `?page=1&pageSize=20` (listeleme endpoint'leri) |
| Versiyon | `/v1/` prefix zorunlu |

### Standart Hata Modeli
```json
{
  "success": false,
  "error": {
    "code": "USER_NOT_FOUND",
    "message": "Kullanıcı bulunamadı.",
    "details": null
  }
}
```

### Standart Başarı Modeli
```json
{
  "success": true,
  "data": { ... },
  "meta": { "page": 1, "pageSize": 20, "total": 150 }
}
```

---

## Domain 1 — Auth (Kimlik Doğrulama)

### 1.1 Giriş Yap
```
POST /auth/login
```

**Request Body:**
```json
{
  "email": "ahmet@okul-a.com",
  "password": "şifre123",
  "rememberMe": true
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
    "expiresIn": 3600,
    "user": {
      "id": "u1",
      "name": "Ahmet Yılmaz",
      "email": "ahmet@okul-a.com",
      "role": "Öğretmen/Antrenör",
      "schoolId": "school-a",
      "avatar": "https://cdn.edutrack.com/avatars/u1.jpg"
    }
  }
}
```

**Response 401:**
```json
{
  "success": false,
  "error": { "code": "INVALID_CREDENTIALS", "message": "E-posta veya şifre hatalı." }
}
```

---

### 1.2 Kayıt Ol
```
POST /auth/register
```

**Request Body:**
```json
{
  "name": "Ahmet Yılmaz",
  "email": "ahmet@okul-a.com",
  "phone": "05321234567",
  "password": "şifre123",
  "kvkkConsent": true
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4...",
    "expiresIn": 3600,
    "user": {
      "id": "u-new-123",
      "name": "Ahmet Yılmaz",
      "email": "ahmet@okul-a.com",
      "role": "Öğrenci",
      "schoolId": null,
      "avatar": null
    }
  }
}
```

**Response 409:**
```json
{
  "success": false,
  "error": { "code": "EMAIL_ALREADY_EXISTS", "message": "Bu e-posta adresi zaten kayıtlı." }
}
```

---

### 1.3 Şifremi Unuttum
```
POST /auth/forgot-password
```

**Request Body:**
```json
{
  "email": "ahmet@okul-a.com"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "message": "Şifre sıfırlama bağlantısı gönderildi.",
    "maskedEmail": "ah***@okul-a.com"
  }
}
```

---

### 1.4 Şifre Sıfırla (Token ile)
```
POST /auth/reset-password
```

**Request Body:**
```json
{
  "token": "reset-token-from-email",
  "newPassword": "yeniŞifre123"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Şifre başarıyla güncellendi." }
}
```

---

### 1.5 Token Yenile
```
POST /auth/refresh-token
```

**Request Body:**
```json
{
  "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresIn": 3600
  }
}
```

---

### 1.6 Çıkış Yap
```
POST /auth/logout
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Oturum kapatıldı." }
}
```

---

### 1.7 Google ile Giriş
```
POST /auth/google
```

**Request Body:**
```json
{
  "idToken": "google-id-token-from-client"
}
```

**Response 200:** _(1.1 ile aynı structure)_

---

### 1.8 Facebook ile Giriş
```
POST /auth/facebook
```

**Request Body:**
```json
{
  "accessToken": "facebook-access-token-from-client"
}
```

**Response 200:** _(1.1 ile aynı structure)_

---

## Domain 2 — Users (Kullanıcılar)

> **Roller:** `Öğrenci`, `Öğretmen/Antrenör`, `Okul Yöneticisi`, `Veli`, `Sistem Yöneticisi`

### 2.1 Oturumdaki Kullanıcıyı Getir
```
GET /users/me
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "u2",
    "name": "Mehmet Kaya",
    "email": "mehmet@okul-a.com",
    "role": "Öğrenci",
    "schoolId": "school-a",
    "avatar": "https://cdn.edutrack.com/avatars/u2.jpg",
    "phoneNumber": "0505 987 65 43",
    "birthDate": "2008-10-12",
    "gender": "Erkek",
    "address": "Ankara, Türkiye",
    "bio": null,
    "branchIds": ["b1", "b2"],
    "parentIds": ["u3"],
    "childIds": null,
    "badges": ["bg1", "bg2", "bg3"],
    "profileCompletion": 87
  }
}
```

---

### 2.2 Kullanıcı Profili Güncelle
```
PATCH /users/me
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "name": "Mehmet Kaya",
  "phoneNumber": "0505 987 65 43",
  "birthDate": "2008-10-12",
  "gender": "Erkek",
  "address": "Ankara, Türkiye",
  "bio": "EduTrack öğrencisiyim."
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "u2",
    "name": "Mehmet Kaya",
    "phoneNumber": "0505 987 65 43",
    "birthDate": "2008-10-12",
    "gender": "Erkek",
    "address": "Ankara, Türkiye",
    "bio": "EduTrack öğrencisiyim.",
    "profileCompletion": 100
  }
}
```

---

### 2.3 Profil Fotoğrafı Yükle
```
POST /users/me/avatar
Authorization: Bearer <token>
Content-Type: multipart/form-data
```

**Request Body (form-data):**
```
avatar: <binary file>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "avatarUrl": "https://cdn.edutrack.com/avatars/u2-1234567890.jpg"
  }
}
```

---

### 2.4 Şifre Güncelle
```
PATCH /users/me/password
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "currentPassword": "eskiŞifre",
  "newPassword": "yeniŞifre123"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Şifre güncellendi." }
}
```

---

### 2.5 Belirli Kullanıcıyı Getir (ID ile)
```
GET /users/{userId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "u1",
    "name": "Ahmet Yılmaz",
    "email": "ahmet@okul-a.com",
    "role": "Öğretmen/Antrenör",
    "schoolId": "school-a",
    "avatar": "https://cdn.edutrack.com/avatars/u1.jpg",
    "bio": "15 yıllık profesyonel futbol antrenörlüğü tecrübesi.",
    "phoneNumber": "0532 123 45 67",
    "badges": ["bg4", "bg5"]
  }
}
```

---

### 2.6 Okul Kullanıcılarını Listele
```
GET /schools/{schoolId}/users
Authorization: Bearer <token>
Query Params: ?role=Öğrenci&page=1&pageSize=20
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "u2",
      "name": "Mehmet Kaya",
      "email": "mehmet@okul-a.com",
      "role": "Öğrenci",
      "avatar": "https://cdn.edutrack.com/avatars/u2.jpg"
    },
    {
      "id": "u9",
      "name": "Ali Vural",
      "email": "ali@okul-a.com",
      "role": "Öğrenci",
      "avatar": "https://cdn.edutrack.com/avatars/u9.jpg"
    }
  ],
  "meta": { "page": 1, "pageSize": 20, "total": 45 }
}
```

---

### 2.7 Kullanıcı Rolünü Güncelle _(Sistem Yöneticisi)_
```
PATCH /users/{userId}/role
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "role": "Öğretmen/Antrenör"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "u2",
    "role": "Öğretmen/Antrenör",
    "updatedAt": "2026-04-30T10:00:00Z"
  }
}
```

---

### 2.8 Aile Bağlantılarını Getir
```
GET /users/me/family
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "u3",
      "name": "Ayşe Demir",
      "email": "ayse@veli.com",
      "role": "Veli",
      "avatar": "https://cdn.edutrack.com/avatars/u3.jpg",
      "relationshipType": "parent",
      "customRoleLabel": null
    }
  ]
}
```

---

### 2.9 Aile Bağlantısı Ekle
```
POST /users/me/family
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "targetUserId": "u3",
  "relationshipType": "parent",
  "customRoleLabel": "Anne"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Aile bağlantısı eklendi." }
}
```

---

### 2.10 Aile Bağlantısını Kaldır
```
DELETE /users/me/family/{targetUserId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Aile bağlantısı kaldırıldı." }
}
```

---

## Domain 3 — Schools (Okullar / Kurumlar)

### 3.1 Tüm Okulları Listele _(Sistem Yöneticisi)_
```
GET /schools
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "school-a",
      "name": "Kuzey Yıldızı Koleji",
      "location": "İstanbul, Beşiktaş",
      "studentCount": 120,
      "courseCount": 8,
      "imageUrl": "https://cdn.edutrack.com/schools/school-a.jpg"
    },
    {
      "id": "school-b",
      "name": "Güney Işığı Spor Akademisi",
      "location": "Ankara, Çankaya",
      "studentCount": 85,
      "courseCount": 5,
      "imageUrl": "https://cdn.edutrack.com/schools/school-b.jpg"
    }
  ],
  "meta": { "page": 1, "pageSize": 20, "total": 2 }
}
```

---

### 3.2 Okul Detayını Getir
```
GET /schools/{schoolId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "school-a",
    "name": "Kuzey Yıldızı Koleji",
    "location": "İstanbul, Beşiktaş",
    "studentCount": 120,
    "courseCount": 8,
    "imageUrl": "https://cdn.edutrack.com/schools/school-a.jpg",
    "createdAt": "2024-01-01T00:00:00Z"
  }
}
```

---

### 3.3 Okul Oluştur _(Sistem Yöneticisi)_
```
POST /schools
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "name": "Yeni Spor Akademisi",
  "location": "İzmir, Bornova",
  "imageUrl": "https://cdn.edutrack.com/schools/default.jpg"
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": "school-xyz",
    "name": "Yeni Spor Akademisi",
    "location": "İzmir, Bornova",
    "studentCount": 0,
    "courseCount": 0,
    "imageUrl": "https://cdn.edutrack.com/schools/default.jpg",
    "createdAt": "2026-04-30T10:00:00Z"
  }
}
```

---

### 3.4 Okul Güncelle _(Sistem Yöneticisi)_
```
PUT /schools/{schoolId}
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "name": "Kuzey Yıldızı Spor Kompleksi",
  "location": "İstanbul, Beşiktaş",
  "imageUrl": "https://cdn.edutrack.com/schools/school-a-new.jpg"
}
```

**Response 200:** _(3.2 ile aynı structure)_

---

### 3.5 Okul Sil _(Sistem Yöneticisi)_
```
DELETE /schools/{schoolId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Okul ve tüm bağlı veriler silindi." }
}
```

---

## Domain 4 — Branches & Categories (Branş ve Kategoriler)

### 4.1 Tüm Branşları Listele
```
GET /branches
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    { "id": "b1", "name": "Futbol" },
    { "id": "b2", "name": "Basketbol" },
    { "id": "b3", "name": "Matematik" },
    { "id": "b4", "name": "Voleybol" },
    { "id": "b5", "name": "Yüzme" }
  ]
}
```

---

### 4.2 Tüm Kategorileri Listele
```
GET /categories
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    { "id": "c1", "name": "U19" },
    { "id": "c2", "name": "U15" },
    { "id": "c3", "name": "Özel Ders" },
    { "id": "c4", "name": "Grup" }
  ]
}
```

---

## Domain 5 — Courses (Kurslar)

### 5.1 Kullanıcının Kurslarını Listele
```
GET /courses/my
Authorization: Bearer <token>
Query Params: ?search=futbol
```

> Öğrenci → studentIds'de yer aldığı kurslar  
> Öğretmen/Antrenör → teacherId'si eşleşen kurslar  
> Okul Yöneticisi & Sistem Yöneticisi → okula bağlı tüm kurslar

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "crs1",
      "schoolId": "school-a",
      "branchId": "b1",
      "branchName": "Futbol",
      "categoryId": "c1",
      "categoryName": "U19",
      "teacherId": "u1",
      "teacherName": "Ahmet Yılmaz",
      "teacherAvatar": "https://cdn.edutrack.com/avatars/u1.jpg",
      "studentCount": 2,
      "title": "U19 Futbol Elit",
      "location": "A Sahası",
      "address": "41.0082, 28.9784",
      "instructorNotes": "Lütfen antrenmana 15 dakika erken gelerek ısınma hareketlerine başlayın.",
      "schedule": [
        { "day": 1, "startTime": "16:00", "endTime": "18:00" },
        { "day": 3, "startTime": "16:00", "endTime": "18:00" }
      ],
      "userRole": "student"
    }
  ]
}
```

---

### 5.2 Okul Kurslarını Listele
```
GET /schools/{schoolId}/courses
Authorization: Bearer <token>
```

**Response 200:** _(5.1 ile aynı structure)_

---

### 5.3 Kurs Detayını Getir
```
GET /courses/{courseId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "crs1",
    "schoolId": "school-a",
    "branchId": "b1",
    "branchName": "Futbol",
    "categoryId": "c1",
    "categoryName": "U19",
    "teacherId": "u1",
    "teacherName": "Ahmet Yılmaz",
    "teacherAvatar": "https://cdn.edutrack.com/avatars/u1.jpg",
    "teacherBio": "15 yıllık profesyonel futbol antrenörlüğü tecrübesi.",
    "title": "U19 Futbol Elit",
    "location": "A Sahası",
    "address": "41.0082, 28.9784",
    "instructorNotes": "Lütfen antrenmana 15 dakika erken gelerek ısınma hareketlerine başlayın.",
    "schedule": [
      { "day": 1, "startTime": "16:00", "endTime": "18:00" }
    ],
    "students": [
      {
        "id": "u2",
        "name": "Mehmet Kaya",
        "avatar": "https://cdn.edutrack.com/avatars/u2.jpg",
        "email": "mehmet@okul-a.com"
      }
    ]
  }
}
```

---

### 5.4 Kurs Oluştur _(Yönetici / Öğretmen)_
```
POST /courses
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "schoolId": "school-a",
  "branchId": "b1",
  "categoryId": "c1",
  "teacherId": "u1",
  "title": "U19 Futbol Elit",
  "location": "A Sahası",
  "address": "41.0082, 28.9784",
  "instructorNotes": "Ders notları burada.",
  "schedule": [
    { "day": 1, "startTime": "16:00", "endTime": "18:00" }
  ]
}
```

**Response 201:** _(5.3 ile aynı structure, students: [])_

---

### 5.5 Kurs Güncelle _(Yönetici / Kursun Öğretmeni)_
```
PUT /courses/{courseId}
Authorization: Bearer <token>
```

**Request Body:** _(5.4 ile aynı alanlar, tümü opsiyonel)_

**Response 200:** _(5.3 ile aynı structure)_

---

### 5.6 Kurs Sil _(Yönetici / Kursun Öğretmeni)_
```
DELETE /courses/{courseId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Kurs silindi." }
}
```

---

### 5.7 Kursa Öğrenci Ekle
```
POST /courses/{courseId}/students
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "studentId": "u5"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Öğrenci kursa eklendi.", "studentCount": 3 }
}
```

---

### 5.8 Kurstan Öğrenci Çıkar
```
DELETE /courses/{courseId}/students/{studentId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Öğrenci kurstan çıkarıldı.", "studentCount": 2 }
}
```

---

## Domain 6 — Individual Lessons (Bireysel Dersler)

### 6.1 Bireysel Dersleri Listele
```
GET /individual-lessons/my
Authorization: Bearer <token>
Query Params: ?date=2026-04-30
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "ind-1",
      "title": "Özel Matematik",
      "description": "Türev konusuna hazırlık çalışması.",
      "date": "2026-04-30",
      "time": "14:00",
      "role": "taken",
      "students": [
        { "name": "Fatma Şahin", "email": "fatma@okul-a.com" }
      ]
    }
  ]
}
```

---

### 6.2 Bireysel Ders Oluştur
```
POST /individual-lessons
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "title": "Özel Matematik",
  "description": "Türev konusuna hazırlık çalışması.",
  "date": "2026-04-30",
  "time": "14:00",
  "role": "taken",
  "students": [
    { "name": "Fatma Şahin", "email": "fatma@okul-a.com" }
  ]
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": "ind-new-456",
    "title": "Özel Matematik",
    "description": "Türev konusuna hazırlık çalışması.",
    "date": "2026-04-30",
    "time": "14:00",
    "role": "taken",
    "students": [
      { "name": "Fatma Şahin", "email": "fatma@okul-a.com" }
    ]
  }
}
```

---

### 6.3 Bireysel Ders Sil
```
DELETE /individual-lessons/{lessonId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Ders silindi." }
}
```

---

## Domain 7 — Attendance (Yoklama)

### 7.1 Kursa Ait Yoklama Kayıtlarını Getir
```
GET /attendance/course/{courseId}
Authorization: Bearer <token>
Query Params: ?month=2026-04
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "att-1",
      "courseId": "crs1",
      "date": "2026-04-02",
      "presentStudentIds": ["u2", "u9"],
      "absentStudentIds": []
    },
    {
      "id": "att-2",
      "courseId": "crs1",
      "date": "2026-04-07",
      "presentStudentIds": ["u2"],
      "absentStudentIds": ["u9"]
    }
  ]
}
```

---

### 7.2 Öğrencinin Yoklama Özeti
```
GET /attendance/student/{studentId}/summary
Authorization: Bearer <token>
Query Params: ?courseId=crs1&month=2026-04
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "studentId": "u2",
    "courseId": "crs1",
    "month": "2026-04",
    "totalClasses": 13,
    "attendedClasses": 12,
    "missedClasses": 1,
    "attendanceRate": 92.3,
    "dailyRecords": [
      { "date": "2026-04-02", "status": "present" },
      { "date": "2026-04-07", "status": "absent" },
      { "date": "2026-04-09", "status": "present" }
    ]
  }
}
```

---

### 7.3 Yoklama Kaydet / Güncelle _(Öğretmen)_
```
POST /attendance
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "courseId": "crs1",
  "date": "2026-04-30",
  "presentStudentIds": ["u2", "u9"],
  "absentStudentIds": []
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": "att-new-789",
    "courseId": "crs1",
    "date": "2026-04-30",
    "presentStudentIds": ["u2", "u9"],
    "absentStudentIds": [],
    "recordedBy": "u1",
    "recordedAt": "2026-04-30T16:05:00Z"
  }
}
```

---

### 7.4 Haftalık Aktivite Verisi (Dashboard)
```
GET /attendance/student/{studentId}/weekly
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "weekRange": { "start": "2026-04-24", "end": "2026-04-30" },
    "days": [
      { "date": "2026-04-24", "dayOfWeek": 4, "status": "attended" },
      { "date": "2026-04-25", "dayOfWeek": 5, "status": "attended" },
      { "date": "2026-04-26", "dayOfWeek": 6, "status": "none" },
      { "date": "2026-04-27", "dayOfWeek": 0, "status": "none" },
      { "date": "2026-04-28", "dayOfWeek": 1, "status": "attended" },
      { "date": "2026-04-29", "dayOfWeek": 2, "status": "missed" },
      { "date": "2026-04-30", "dayOfWeek": 3, "status": "attended" }
    ]
  }
}
```

---

### 7.5 Aylık Aktivite Isı Haritası Verisi
```
GET /attendance/student/{studentId}/monthly-heatmap
Authorization: Bearer <token>
Query Params: ?month=2026-04
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "month": "2026-04",
    "totalDays": 30,
    "activeDays": 14,
    "attendanceRate": 92,
    "dailyStatuses": [
      { "day": 1, "status": "none" },
      { "day": 2, "status": "attended" },
      { "day": 7, "status": "absent" },
      { "day": 9, "status": "attended" }
    ]
  }
}
```

---

## Domain 8 — Payments (Ödemeler / Aidatlar)

### 8.1 Kullanıcının Ödemelerini Getir
```
GET /payments/my
Authorization: Bearer <token>
Query Params: ?status=Gecikti
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "pay1",
      "studentId": "u2",
      "studentName": "Mehmet Kaya",
      "courseId": "crs1",
      "courseTitle": "U19 Futbol Elit",
      "amount": 1500,
      "dueDate": "2026-05-01",
      "status": "Gecikti",
      "method": null,
      "paidAt": null
    },
    {
      "id": "pay2",
      "studentId": "u2",
      "studentName": "Mehmet Kaya",
      "courseId": "crs2",
      "courseTitle": "Matematik İleri Seviye",
      "amount": 1200,
      "dueDate": "2026-06-15",
      "status": "Ödendi",
      "method": "Credit Card",
      "paidAt": "2026-06-10"
    }
  ]
}
```

---

### 8.2 Okula Ait Ödemeleri Listele _(Yönetici / Öğretmen)_
```
GET /payments/school/{schoolId}
Authorization: Bearer <token>
Query Params: ?status=Gecikti&page=1&pageSize=20
```

**Response 200:** _(8.1 ile aynı structure + meta)_

---

### 8.3 Ödeme Oluştur _(Yönetici)_
```
POST /payments
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "studentId": "u2",
  "courseId": "crs1",
  "amount": 1500,
  "dueDate": "2026-07-01"
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": "pay-new-101",
    "studentId": "u2",
    "courseId": "crs1",
    "amount": 1500,
    "dueDate": "2026-07-01",
    "status": "Onay Bekliyor",
    "method": null,
    "paidAt": null,
    "createdAt": "2026-04-30T10:00:00Z"
  }
}
```

---

### 8.4 Ödeme Durumunu Güncelle _(Yönetici / Ödeme için Onaylar)_
```
PATCH /payments/{paymentId}/status
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "status": "Ödendi",
  "method": "Manual",
  "paidAt": "2026-04-30"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "pay1",
    "status": "Ödendi",
    "method": "Manual",
    "paidAt": "2026-04-30"
  }
}
```

---

### 8.5 Online Ödeme Başlat _(Öğrenci / Veli)_
```
POST /payments/{paymentId}/pay
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "method": "Credit Card",
  "cardToken": "tok_visa_from_payment_gateway"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "paymentId": "pay1",
    "transactionId": "txn-abc-123",
    "status": "Ödendi",
    "paidAt": "2026-04-30T11:30:00Z"
  }
}
```

---

### 8.6 Gecikmiş Ödemelere Toplu Hatırlatma Gönder _(Yönetici / Öğretmen)_
```
POST /payments/remind-overdue
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "schoolId": "school-a"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "notifiedCount": 3,
    "message": "3 kullanıcıya ödeme hatırlatması gönderildi."
  }
}
```

---

## Domain 9 — Notifications (Bildirimler)

### 9.1 Kullanıcının Bildirimlerini Getir
```
GET /notifications
Authorization: Bearer <token>
Query Params: ?isRead=false&page=1&pageSize=20
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "n1",
      "type": "Ders Hatırlatıcısı",
      "title": "Ders Başlıyor",
      "message": "U19 Futbol Elit Grubu dersi 15 dakika içinde başlayacak.",
      "timestamp": "2026-04-30T15:45:00Z",
      "isRead": false,
      "senderRole": null
    },
    {
      "id": "n3",
      "type": "Okul Duyurusu",
      "title": "Haftalık Program Güncellemesi",
      "message": "Antrenman saatlerinde düzenleme yapılmıştır.",
      "timestamp": "2026-04-30T09:00:00Z",
      "isRead": false,
      "senderRole": "Okul Yöneticisi"
    }
  ],
  "meta": { "page": 1, "pageSize": 20, "total": 5, "unreadCount": 2 }
}
```

---

### 9.2 Bildirimi Okundu Olarak İşaretle
```
PATCH /notifications/{notificationId}/read
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "id": "n1", "isRead": true }
}
```

---

### 9.3 Tüm Bildirimleri Okundu İşaretle
```
PATCH /notifications/read-all
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "updatedCount": 5 }
}
```

---

### 9.4 Duyuru / Bildirim Gönder _(Öğretmen / Yönetici)_
```
POST /notifications/send
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "type": "Okul Duyurusu",
  "title": "Haftalık Program Güncellemesi",
  "message": "Antrenman saatlerinde düzenleme yapılmıştır.",
  "targetAudience": {
    "scope": "school",
    "schoolId": "school-a",
    "roles": ["Öğrenci", "Veli"]
  }
}
```

> `targetAudience.scope` şu değerleri alabilir: `"all"`, `"school"`, `"course"`, `"user"`  
> `scope = "course"` ise `courseId` alanı eklenmeli.  
> `scope = "user"` ise `userIds: ["u2", "u9"]` alanı eklenmeli.

**Response 201:**
```json
{
  "success": true,
  "data": {
    "sentCount": 45,
    "message": "Duyuru 45 kullanıcıya gönderildi."
  }
}
```

---

### 9.5 Ödeme Hatırlatması Gönder _(Yönetici / Öğretmen)_
```
POST /notifications/payment-reminder
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "paymentId": "pay1",
  "studentId": "u2"
}
```

**Response 201:**
```json
{
  "success": true,
  "data": { "message": "Ödeme hatırlatması gönderildi." }
}
```

---

## Domain 10 — Goals & Tasks (Hedefler ve Görevler)

### 10.1 Hedefleri Listele
```
GET /goals
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "g1",
      "title": "Kondisyon Gelişimi",
      "category": "sport",
      "progress": 75,
      "tasks": [
        { "id": "t1", "text": "Haftalık 3 gün antrenman", "isCompleted": true },
        { "id": "t2", "text": "10 km koşu hedefi", "isCompleted": false },
        { "id": "t3", "text": "Esneklik egzersizleri", "isCompleted": false },
        { "id": "t4", "text": "Beslenme planına uymak", "isCompleted": true }
      ],
      "createdAt": "2026-04-01T00:00:00Z"
    }
  ]
}
```

---

### 10.2 Hedef Oluştur
```
POST /goals
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "title": "Akademik Başarı",
  "category": "academic"
}
```

> `category` şu değerleri alabilir: `"sport"`, `"academic"`, `"personal"`

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": "g-new-456",
    "title": "Akademik Başarı",
    "category": "academic",
    "progress": 0,
    "tasks": [],
    "createdAt": "2026-04-30T10:00:00Z"
  }
}
```

---

### 10.3 Hedefe Görev Ekle
```
POST /goals/{goalId}/tasks
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "text": "Her gün 30 dakika çalışmak"
}
```

**Response 201:**
```json
{
  "success": true,
  "data": {
    "id": "t-new-789",
    "text": "Her gün 30 dakika çalışmak",
    "isCompleted": false
  }
}
```

---

### 10.4 Görevi Tamamlandı / Tamamlanmadı Yap
```
PATCH /goals/{goalId}/tasks/{taskId}/toggle
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "id": "t1",
    "isCompleted": false,
    "goalProgress": 50
  }
}
```

---

### 10.5 Hedef Sil
```
DELETE /goals/{goalId}
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Hedef ve tüm görevler silindi." }
}
```

---

## Domain 11 — Badges (Rozetler / Başarılar)

### 11.1 Tüm Sistem Rozetlerini Getir
```
GET /badges
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": [
    {
      "id": "bg1",
      "name": "7 Günlük Seri",
      "description": "Bir hafta boyunca her gün antrenmana katıldın. Muhteşem süreklilik!",
      "icon": "🔥",
      "color": "from-orange-400 to-rose-500",
      "criteria": {
        "type": "attendance_streak",
        "requiredDays": 7
      }
    },
    {
      "id": "bg2",
      "name": "Erken Kuş",
      "description": "Sabah 09:00 öncesindeki antrenmanların yıldızı sensin.",
      "icon": "🌅",
      "color": "from-amber-300 to-orange-500",
      "criteria": {
        "type": "early_attendance",
        "beforeHour": 9
      }
    }
  ]
}
```

---

### 11.2 Kullanıcının Rozetlerini Getir
```
GET /users/{userId}/badges
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "earned": [
      { "id": "bg1", "name": "7 Günlük Seri", "icon": "🔥", "color": "from-orange-400 to-rose-500", "earnedAt": "2026-04-15T00:00:00Z" }
    ],
    "locked": [
      { "id": "bg3", "name": "Çift Mesai", "icon": "⚡", "color": "from-indigo-400 to-purple-600" }
    ]
  }
}
```

---

### 11.3 Rozet Ver _(Sistem / Otomatik Trigger)_
```
POST /users/{userId}/badges
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "badgeId": "bg1"
}
```

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Rozet başarıyla verildi.", "badgeId": "bg1" }
}
```

---

## Domain 12 — Analytics (İstatistikler)

### 12.1 Aylık Aktivite Özeti
```
GET /analytics/students/{studentId}/monthly
Authorization: Bearer <token>
Query Params: ?month=2026-04
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "studentId": "u2",
    "month": "2026-04",
    "totalLessons": 14,
    "attendedLessons": 13,
    "missedLessons": 1,
    "attendanceRate": 92.8,
    "totalHours": 28,
    "activeCourseCount": 2
  }
}
```

---

### 12.2 Profil Tamamlanma Oranı
```
GET /analytics/users/{userId}/profile-completion
Authorization: Bearer <token>
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "percentage": 87,
    "completedCount": 7,
    "totalCount": 8,
    "fields": [
      { "key": "name", "label": "Ad Soyad", "isCompleted": true },
      { "key": "email", "label": "E-Posta", "isCompleted": true },
      { "key": "phoneNumber", "label": "Telefon", "isCompleted": true },
      { "key": "avatar", "label": "Fotoğraf", "isCompleted": true },
      { "key": "birthDate", "label": "Doğum Tarihi", "isCompleted": true },
      { "key": "gender", "label": "Cinsiyet", "isCompleted": true },
      { "key": "address", "label": "Adres", "isCompleted": true },
      { "key": "bio", "label": "Hakkında", "isCompleted": false }
    ]
  }
}
```

---

### 12.3 Okul Finansal Özeti _(Yönetici)_
```
GET /analytics/schools/{schoolId}/financials
Authorization: Bearer <token>
Query Params: ?month=2026-04
```

**Response 200:**
```json
{
  "success": true,
  "data": {
    "schoolId": "school-a",
    "month": "2026-04",
    "totalPayments": 12,
    "paidCount": 8,
    "overdueCount": 3,
    "pendingCount": 1,
    "totalCollected": 9600,
    "totalOverdue": 4500,
    "collectionRate": 68.1
  }
}
```

---

## Rol Tabanlı Yetki Matrisi

| Endpoint Grubu | Öğrenci | Öğretmen | Okul Yöneticisi | Sistem Yöneticisi | Veli |
|---|:---:|:---:|:---:|:---:|:---:|
| Kendi profilini görme/güncelleme | ✅ | ✅ | ✅ | ✅ | ✅ |
| Başka kullanıcı profilini görme | ✅ | ✅ | ✅ | ✅ | ✅* |
| Kullanıcı rolü değiştirme | ❌ | ❌ | ✅† | ✅ | ❌ |
| Okul CRUD | ❌ | ❌ | ❌ | ✅ | ❌ |
| Kurs listeleme | ✅ | ✅ | ✅ | ✅ | ✅* |
| Kurs oluşturma/güncelleme/silme | ❌ | ✅‡ | ✅ | ✅ | ❌ |
| Yoklama kaydetme | ❌ | ✅ | ✅ | ✅ | ❌ |
| Kendi yoklamasını görme | ✅ | ✅ | ✅ | ✅ | ✅* |
| Bildirim gönderme | ❌ | ✅ | ✅ | ✅ | ❌ |
| Ödeme görme | ✅ | ✅‡ | ✅ | ✅ | ✅* |
| Ödeme oluşturma | ❌ | ❌ | ✅ | ✅ | ❌ |
| Ödeme onaylama | ❌ | ❌ | ✅ | ✅ | ❌ |
| Online ödeme yapma | ✅ | ✅ | ✅ | ✅ | ✅* |
| Rozet verme | ❌ | ❌ | ❌ | ✅ (sistem) | ❌ |
| Hedef/görev yönetimi | ✅ | ✅ | ✅ | ✅ | ❌ |

> \* Veli yalnızca kendi çocuklarının verilerine erişebilir.  
> † Okul Yöneticisi yalnızca `Öğrenci → Öğretmen/Antrenör` rol yükseltmesi yapabilir.  
> ‡ Öğretmen yalnızca TeacherId'si kendisi olan kursları yönetebilir, ödemeleri yalnızca kendi kurslarındaki öğrenciler için görebilir.

---

## Push Notification (FCM/APNs) Entegrasyonu

### Cihaz Token Kaydet
```
POST /users/me/device-token
Authorization: Bearer <token>
```

**Request Body:**
```json
{
  "token": "fcm-device-token-xyz",
  "platform": "android"
}
```

> `platform` şu değerleri alabilir: `"ios"`, `"android"`, `"web"`

**Response 200:**
```json
{
  "success": true,
  "data": { "message": "Cihaz token kaydedildi." }
}
```

---

## Domain Modelleri Özet (Clean Architecture - Domain Layer)

### User Aggregate
```
User
├── id: string
├── name: string
├── email: string (unique)
├── passwordHash: string
├── role: UserRole (enum)
├── schoolId?: string → FK School
├── avatar?: string (CDN URL)
├── phoneNumber?: string
├── birthDate?: date
├── gender?: enum(Erkek, Kadın, Belirtilmedi)
├── address?: string
├── bio?: string
├── branchIds?: string[]
├── childIds?: string[] → FK User (Öğrenci)
├── parentIds?: string[] → FK User (Veli)
├── badges?: string[] → FK Badge
├── deviceTokens?: string[]
├── createdAt: datetime
└── updatedAt: datetime
```

### School Aggregate
```
School
├── id: string
├── name: string
├── location: string
├── imageUrl?: string
├── createdAt: datetime
└── updatedAt: datetime
```

### Course Aggregate
```
Course
├── id: string
├── schoolId: string → FK School
├── branchId: string → FK Branch
├── categoryId: string → FK Category
├── teacherId: string → FK User
├── studentIds: string[] → FK User[]
├── title: string
├── location?: string
├── address?: string (koordinat veya adres metni)
├── instructorNotes?: string
├── schedule: ScheduleEntry[]
│   ├── day: int (0=Pazar, 6=Cumartesi)
│   ├── startTime: string (HH:mm)
│   └── endTime: string (HH:mm)
├── createdAt: datetime
└── updatedAt: datetime
```

### AttendanceRecord Aggregate
```
AttendanceRecord
├── id: string
├── courseId: string → FK Course
├── date: date (YYYY-MM-DD)
├── presentStudentIds: string[] → FK User[]
├── absentStudentIds: string[] → FK User[]
├── recordedBy: string → FK User
└── recordedAt: datetime
```

### PaymentRecord Aggregate
```
PaymentRecord
├── id: string
├── studentId: string → FK User
├── courseId?: string → FK Course
├── amount: decimal
├── dueDate: date
├── status: PaymentStatus (enum: Ödendi, Gecikti, Onay Bekliyor)
├── method?: enum(Credit Card, Manual)
├── transactionId?: string
├── paidAt?: date
├── createdAt: datetime
└── updatedAt: datetime
```

### Notification Aggregate
```
Notification
├── id: string
├── recipientId: string → FK User
├── type: NotificationType (enum)
├── title: string
├── message: string
├── isRead: boolean
├── senderUserId?: string → FK User
├── senderRole?: UserRole
└── createdAt: datetime (= timestamp)
```

### IndividualLesson Aggregate
```
IndividualLesson
├── id: string
├── createdByUserId: string → FK User
├── title: string
├── description: string
├── date: date (YYYY-MM-DD)
├── time: string (HH:mm)
├── role: enum(taken, given)
├── students: LessonParticipant[]
│   ├── name: string
│   └── email: string
├── createdAt: datetime
└── updatedAt: datetime
```

### Goal & Task Aggregates
```
Goal
├── id: string
├── userId: string → FK User
├── title: string
├── category: enum(sport, academic, personal)
├── tasks: Task[]
├── createdAt: datetime
└── updatedAt: datetime

Task
├── id: string
├── goalId: string → FK Goal
├── text: string
├── isCompleted: boolean
└── updatedAt: datetime
```

### Badge Value Object
```
Badge
├── id: string
├── name: string
├── description: string
├── icon: string (emoji)
├── color: string (Tailwind gradient)
└── criteria: BadgeCriteria (JSON - trigger logic için)
```

---

## Önerilen Clean Architecture Katmanları

```
src/
├── Domain/
│   ├── Entities/       (User, Course, School, ...)
│   ├── Enums/          (UserRole, PaymentStatus, NotificationType, ...)
│   ├── Interfaces/     (IUserRepository, ICourseRepository, ...)
│   └── ValueObjects/   (Email, Money, ScheduleEntry, ...)
│
├── Application/
│   ├── Users/
│   │   ├── Commands/   (UpdateProfileCommand, ChangePasswordCommand, ...)
│   │   └── Queries/    (GetCurrentUserQuery, GetUserByIdQuery, ...)
│   ├── Courses/
│   ├── Attendance/
│   ├── Payments/
│   ├── Notifications/
│   ├── Goals/
│   └── Analytics/
│
├── Infrastructure/
│   ├── Persistence/    (DbContext, Repositories)
│   ├── Identity/       (JWT, OAuth handlers)
│   ├── Messaging/      (FCM/APNs push notification)
│   ├── Storage/        (CDN avatar upload)
│   └── Payment/        (Gateway integration)
│
└── Presentation/
    └── API/
        ├── Controllers/
        ├── Middlewares/ (Auth, Error handling)
        └── DTOs/
```
