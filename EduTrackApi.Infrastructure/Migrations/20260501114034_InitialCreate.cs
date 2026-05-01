using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EduTrackApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "edutrack");

            migrationBuilder.CreateTable(
                name: "badges",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    icon = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    color = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badges", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "branches",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_branches", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_types",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_statuses",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "schools",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_schools", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    type_id = table.Column<short>(type: "smallint", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    sender_role_id = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_notification_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "edutrack",
                        principalTable: "notification_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_notifications_user_roles_sender_role_id",
                        column: x => x.sender_role_id,
                        principalSchema: "edutrack",
                        principalTable: "user_roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role_id = table.Column<short>(type: "smallint", nullable: false),
                    school_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    avatar = table.Column<string>(type: "text", nullable: true),
                    bio = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    gender = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    refresh_token = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    refresh_token_expiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_schools_school_id",
                        column: x => x.school_id,
                        principalSchema: "edutrack",
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_users_user_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "edutrack",
                        principalTable: "user_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    school_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    branch_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    category_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    teacher_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    location = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    instructor_notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                    table.ForeignKey(
                        name: "FK_courses_branches_branch_id",
                        column: x => x.branch_id,
                        principalSchema: "edutrack",
                        principalTable: "branches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courses_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "edutrack",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courses_schools_school_id",
                        column: x => x.school_id,
                        principalSchema: "edutrack",
                        principalTable: "schools",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_courses_users_teacher_id",
                        column: x => x.teacher_id,
                        principalSchema: "edutrack",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parent_children",
                schema: "edutrack",
                columns: table => new
                {
                    parent_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    child_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parent_children", x => new { x.parent_id, x.child_id });
                    table.ForeignKey(
                        name: "FK_parent_children_users_child_id",
                        column: x => x.child_id,
                        principalSchema: "edutrack",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_parent_children_users_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "edutrack",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_badges",
                schema: "edutrack",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    badge_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    awarded_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_badges", x => new { x.user_id, x.badge_id });
                    table.ForeignKey(
                        name: "FK_user_badges_badges_badge_id",
                        column: x => x.badge_id,
                        principalSchema: "edutrack",
                        principalTable: "badges",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_badges_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "edutrack",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_schedules",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    day_of_week = table.Column<short>(type: "smallint", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_schedules", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_schedules_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "edutrack",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course_students",
                schema: "edutrack",
                columns: table => new
                {
                    course_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    student_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course_students", x => new { x.course_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_course_students_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "edutrack",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_course_students_users_student_id",
                        column: x => x.student_id,
                        principalSchema: "edutrack",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "edutrack",
                columns: table => new
                {
                    id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    student_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    course_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status_id = table.Column<short>(type: "smallint", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    method = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_courses_course_id",
                        column: x => x.course_id,
                        principalSchema: "edutrack",
                        principalTable: "courses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_payment_statuses_status_id",
                        column: x => x.status_id,
                        principalSchema: "edutrack",
                        principalTable: "payment_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_payments_users_student_id",
                        column: x => x.student_id,
                        principalSchema: "edutrack",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "badges",
                columns: new[] { "id", "color", "description", "icon", "name" },
                values: new object[,]
                {
                    { "bg1", "from-orange-400 to-rose-500", "Bir hafta boyunca her gün antrenmana katıldın. Muhteşem süreklilik!", "🔥", "7 Günlük Seri" },
                    { "bg2", "from-amber-300 to-orange-500", "Sabah 09:00 öncesindeki antrenmanların yıldızı sensin.", "🌅", "Erken Kuş" },
                    { "bg3", "from-indigo-400 to-purple-600", "Aynı gün içerisinde iki farklı branşta eğitim alarak sınırları zorladın.", "⚡", "Çift Mesai" },
                    { "bg4", "from-emerald-400 to-teal-600", "Bu ay toplamda 20 saati aşkın antrenman süresine ulaştın.", "🎯", "Sadık Sporcu" },
                    { "bg5", "from-blue-400 to-indigo-600", "Eğitmen notlarında üst üste 3 kez \"Üstün Başarı\" yorumu aldın.", "🚀", "Gelişim Öncüsü" },
                    { "bg6", "from-pink-400 to-rose-500", "Grup çalışmalarında en çok yardımlaşan öğrenci seçildin.", "🤝", "Sosyal Kelebek" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "branches",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { "b1", "Futbol" },
                    { "b2", "Basketbol" },
                    { "b3", "Matematik" },
                    { "b4", "Voleybol" },
                    { "b5", "Yüzme" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { "c1", "U19" },
                    { "c2", "U15" },
                    { "c3", "Özel Ders" },
                    { "c4", "Grup" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "notification_types",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { (short)1, "ANNOUNCEMENT", "Duyuru" },
                    { (short)2, "UPCOMING_CLASS", "Yaklaşan Ders" },
                    { (short)3, "ATTENDANCE_UPDATE", "Yoklama Güncellemesi" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "payment_statuses",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { (short)1, "PENDING", "Beklemede" },
                    { (short)2, "PAID", "Ödendi" },
                    { (short)3, "OVERDUE", "Gecikmiş" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "schools",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { "school-a", "Okul A" },
                    { "school-b", "Okul B" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "user_roles",
                columns: new[] { "id", "code", "name" },
                values: new object[,]
                {
                    { (short)1, "SYSTEM_ADMIN", "Sistem Yöneticisi" },
                    { (short)2, "SCHOOL_ADMIN", "Okul Yöneticisi" },
                    { (short)3, "TEACHER", "Eğitmen" },
                    { (short)4, "PARENT", "Veli" },
                    { (short)5, "STUDENT", "Öğrenci" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "notifications",
                columns: new[] { "id", "is_read", "message", "sender_role_id", "timestamp", "title", "type_id" },
                values: new object[,]
                {
                    { "n1", false, "U19 Futbol Elit Grubu dersi 15 dakika içinde başlayacak.", null, new DateTime(2024, 4, 1, 11, 30, 0, 0, DateTimeKind.Utc), "Ders Başlıyor", (short)2 },
                    { "n2", true, "Mehmet Kaya bugünkü matematik dersinde \"Var\" olarak işaretlendi.", null, new DateTime(2024, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc), "Yoklama Alındı", (short)3 },
                    { "n3", false, "Antrenman saatlerinde düzenleme yapılmıştır.", (short)2, new DateTime(2024, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Haftalık Program Güncellemesi", (short)1 }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "users",
                columns: new[] { "id", "address", "avatar", "bio", "birth_date", "email", "gender", "name", "password_hash", "phone_number", "refresh_token", "refresh_token_expiry", "role_id", "school_id" },
                values: new object[,]
                {
                    { "admin", "İstanbul, Türkiye", "https://picsum.photos/seed/admin/200", null, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), "admin@edutrack.com", "Kadın", "Zeynep Sistem", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", "0555 111 22 33", null, null, (short)1, null },
                    { "u1", null, "https://picsum.photos/seed/u1/200", "15 yıllık profesyonel futbol antrenörlüğü tecrübesi.", null, "ahmet@okul-a.com", null, "Ahmet Yılmaz", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", "0532 123 45 67", null, null, (short)3, "school-a" },
                    { "u2", "Ankara, Türkiye", "https://picsum.photos/seed/u2/200", null, new DateTime(2008, 10, 12, 0, 0, 0, 0, DateTimeKind.Utc), "mehmet@okul-a.com", "Erkek", "Mehmet Kaya", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", "0505 987 65 43", null, null, (short)5, "school-a" },
                    { "u3", null, "https://picsum.photos/seed/u3/200", null, null, "ayse@veli.com", null, "Ayşe Demir", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", null, null, null, (short)4, null },
                    { "u4", null, "https://picsum.photos/seed/u4/200", null, null, "canan@okul-a.com", null, "Canan Sert", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", "0555 444 55 66", null, null, (short)2, "school-a" },
                    { "u5", null, "https://picsum.photos/seed/u5/200", null, null, "bulent@okul-b.com", null, "Bülent Arın", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", null, null, null, (short)2, "school-b" },
                    { "u7", null, "https://picsum.photos/seed/u7/200", "Matematik Olimpiyatları koordinatörü.", null, "fatma@okul-a.com", null, "Fatma Şahin", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", null, null, null, (short)3, "school-a" },
                    { "u8", null, "https://picsum.photos/seed/u8/200", null, null, "murat@okul-a.com", null, "Murat Can", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", null, null, null, (short)3, "school-a" },
                    { "u9", null, "https://picsum.photos/seed/u9/200", null, null, "ali@okul-a.com", null, "Ali Vural", "$2a$11$K3xOGa7FBLFHgSWVbymq7OZHnEFgPD4./uwvxMz6E9GZ1q3dBp3Qy", null, null, null, (short)5, "school-a" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "courses",
                columns: new[] { "id", "address", "branch_id", "category_id", "instructor_notes", "location", "school_id", "teacher_id", "title" },
                values: new object[,]
                {
                    { "crs1", "41.0082, 28.9784", "b1", "c1", "Lütfen antrenmana 15 dakika erken gelerek ısınma hareketlerine başlayın. Krampon kontrolü yapılacak.", "A Sahası", "school-a", "u1", "U19 Futbol Elit" },
                    { "crs2", "Ankara, Çankaya", "b3", "c3", "Geçen haftaki problem setini yanınızda getirmeyi unutmayın. Türev konusuna giriş yapacağız.", "Z-12 Laboratuvarı", "school-a", "u7", "Matematik İleri Seviye" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "parent_children",
                columns: new[] { "child_id", "parent_id" },
                values: new object[,]
                {
                    { "u2", "u3" },
                    { "u9", "u3" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "user_badges",
                columns: new[] { "badge_id", "user_id", "awarded_at" },
                values: new object[,]
                {
                    { "bg1", "u2", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "bg2", "u2", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "bg3", "u2", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "bg4", "u2", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "bg5", "u2", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "bg1", "u9", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "course_schedules",
                columns: new[] { "id", "course_id", "day_of_week", "end_time", "start_time" },
                values: new object[,]
                {
                    { 1L, "crs1", (short)1, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0) },
                    { 2L, "crs1", (short)4, new TimeSpan(0, 18, 0, 0, 0), new TimeSpan(0, 16, 0, 0, 0) },
                    { 3L, "crs2", (short)1, new TimeSpan(0, 20, 0, 0, 0), new TimeSpan(0, 18, 30, 0, 0) },
                    { 4L, "crs2", (short)3, new TimeSpan(0, 20, 0, 0, 0), new TimeSpan(0, 18, 30, 0, 0) }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "course_students",
                columns: new[] { "course_id", "student_id" },
                values: new object[,]
                {
                    { "crs1", "u2" },
                    { "crs1", "u9" },
                    { "crs2", "u2" },
                    { "crs2", "u9" }
                });

            migrationBuilder.InsertData(
                schema: "edutrack",
                table: "payments",
                columns: new[] { "id", "amount", "course_id", "due_date", "method", "paid_at", "status_id", "student_id" },
                values: new object[,]
                {
                    { "pay1", 1500m, "crs1", new DateTime(2024, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, (short)3, "u2" },
                    { "pay2", 1200m, "crs2", new DateTime(2024, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Credit Card", new DateTime(2024, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), (short)2, "u2" },
                    { "pay3", 1500m, "crs1", new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manual", null, (short)1, "u2" },
                    { "pay4", 1500m, "crs1", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Manual", new DateTime(2024, 5, 28, 0, 0, 0, 0, DateTimeKind.Utc), (short)2, "u9" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_course_schedules_course_id",
                schema: "edutrack",
                table: "course_schedules",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_students_student_id",
                schema: "edutrack",
                table: "course_students",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_branch_id",
                schema: "edutrack",
                table: "courses",
                column: "branch_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_category_id",
                schema: "edutrack",
                table: "courses",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_school_id",
                schema: "edutrack",
                table: "courses",
                column: "school_id");

            migrationBuilder.CreateIndex(
                name: "IX_courses_teacher_id",
                schema: "edutrack",
                table: "courses",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_sender_role_id",
                schema: "edutrack",
                table: "notifications",
                column: "sender_role_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_type",
                schema: "edutrack",
                table: "notifications",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_parent_children_child_id",
                schema: "edutrack",
                table: "parent_children",
                column: "child_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_status_id",
                schema: "edutrack",
                table: "payments",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_course",
                schema: "edutrack",
                table: "payments",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_payments_student",
                schema: "edutrack",
                table: "payments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_badges_badge_id",
                schema: "edutrack",
                table: "user_badges",
                column: "badge_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "edutrack",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                schema: "edutrack",
                table: "users",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_school_id",
                schema: "edutrack",
                table: "users",
                column: "school_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "course_schedules",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "course_students",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "parent_children",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "user_badges",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "notification_types",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "courses",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "payment_statuses",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "badges",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "branches",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "users",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "schools",
                schema: "edutrack");

            migrationBuilder.DropTable(
                name: "user_roles",
                schema: "edutrack");
        }
    }
}
