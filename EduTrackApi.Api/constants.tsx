
import { UserRole, Branch, Category, User, Course, Notification, NotificationType, Badge, PaymentRecord, PaymentStatus } from './types.ts';

export const INITIAL_BRANCHES: Branch[] = [
  { id: 'b1', name: 'Football' },
  { id: 'b2', name: 'Basketball' },
  { id: 'b3', name: 'Mathematics' },
  { id: 'b4', name: 'Volleyball' },
  { id: 'b5', name: 'Swimming' }
];

export const INITIAL_CATEGORIES: Category[] = [
  { id: 'c1', name: 'U19' },
  { id: 'c2', name: 'U15' },
  { id: 'c3', name: 'Private Lesson' },
  { id: 'c4', name: 'Group' }
];

export const MOCK_PAYMENTS: PaymentRecord[] = [
  { id: 'pay1', studentId: 'u2', courseId: 'crs1', amount: 1500, dueDate: '2024-05-01', status: PaymentStatus.OVERDUE },
  { id: 'pay2', studentId: 'u2', courseId: 'crs2', amount: 1200, dueDate: '2024-06-15', status: PaymentStatus.PAID, paidAt: '2024-06-10', method: 'Credit Card' },
  { id: 'pay3', studentId: 'u2', courseId: 'crs1', amount: 1500, dueDate: '2024-07-01', status: PaymentStatus.PENDING, method: 'Manual' },
  { id: 'pay4', studentId: 'u9', courseId: 'crs1', amount: 1500, dueDate: '2024-06-01', status: PaymentStatus.PAID, paidAt: '2024-05-28', method: 'Manual' }
];

export const SYSTEM_BADGES: Badge[] = [
  { id: 'bg1', name: '7 Day Streak', description: 'You attended training every day for a week. Amazing consistency!', icon: '🔥', color: 'from-orange-400 to-rose-500' },
  { id: 'bg2', name: 'Early Bird', description: 'You are the star of morning trainings before 09:00.', icon: '🌅', color: 'from-amber-300 to-orange-500' },
  { id: 'bg3', name: 'Double Shift', description: 'You pushed boundaries by training in two different branches on the same day.', icon: '⚡', color: 'from-indigo-400 to-purple-600' },
  { id: 'bg4', name: 'Loyal Athlete', description: 'You reached over 20 hours of training this month.', icon: '🎯', color: 'from-emerald-400 to-teal-600' },
  { id: 'bg5', name: 'Growth Pioneer', description: 'You received "Outstanding Success" comments 3 times in a row from instructor notes.', icon: '🚀', color: 'from-blue-400 to-indigo-600' },
  { id: 'bg6', name: 'Social Butterfly', description: 'You were selected as the most collaborative student in group activities.', icon: '🤝', color: 'from-pink-400 to-rose-500' }
];

export const MOCK_USERS: User[] = [
  { 
    id: 'admin', 
    name: 'Zeynep Sistem', 
    role: UserRole.SYSTEM_ADMIN, 
    email: 'admin@edutrack.com', 
    avatar: 'https://picsum.photos/seed/admin/200',
    phoneNumber: '0555 111 22 33',
    gender: 'Female',
    birthDate: '1990-05-15',
    address: 'Istanbul, Turkey'
  },
  { 
    id: 'u4', 
    name: 'Canan Sert', 
    role: UserRole.SCHOOL_ADMIN, 
    email: 'canan@okul-a.com', 
    schoolId: 'school-a', 
    avatar: 'https://picsum.photos/seed/u4/200',
    phoneNumber: '0555 444 55 66'
  },
  { 
    id: 'u1', 
    name: 'Ahmet Yilmaz', 
    role: UserRole.TEACHER, 
    email: 'ahmet@okul-a.com', 
    schoolId: 'school-a', 
    avatar: 'https://picsum.photos/seed/u1/200', 
    bio: '15 years of professional football coaching experience.',
    phoneNumber: '0532 123 45 67'
  },
  { id: 'u3', name: 'Ayse Demir', role: UserRole.PARENT, email: 'ayse@veli.com', avatar: 'https://picsum.photos/seed/u3/200', childIds: ['u2', 'u9'] },
  { 
    id: 'u2', 
    name: 'Mehmet Kaya', 
    role: UserRole.STUDENT, 
    email: 'mehmet@okul-a.com', 
    schoolId: 'school-a', 
    avatar: 'https://picsum.photos/seed/u2/200', 
    parentIds: ['u3'], 
    badges: ['bg1', 'bg2', 'bg3', 'bg4', 'bg5'],
    phoneNumber: '0505 987 65 43',
    birthDate: '2008-10-12',
    gender: 'Male',
    address: 'Ankara, Turkey'
  },
  { id: 'u9', name: 'Ali Vural', role: UserRole.STUDENT, email: 'ali@okul-a.com', schoolId: 'school-a', avatar: 'https://picsum.photos/seed/u9/200', parentIds: ['u3'], badges: ['bg1'] },
  { id: 'u5', name: 'Bulent Arin', role: UserRole.SCHOOL_ADMIN, email: 'bulent@okul-b.com', schoolId: 'school-b', avatar: 'https://picsum.photos/seed/u5/200' },
  { id: 'u7', name: 'Fatma Sahin', role: UserRole.TEACHER, email: 'fatma@okul-a.com', schoolId: 'school-a', avatar: 'https://picsum.photos/seed/u7/200', bio: 'Mathematics Olympiad coordinator.' },
  { id: 'u8', name: 'Murat Can', role: UserRole.TEACHER, email: 'murat@okul-a.com', schoolId: 'school-a', avatar: 'https://picsum.photos/seed/u8/200' }
];

const today = new Date().getDay();

export const MOCK_COURSES: Course[] = [
  {
    id: 'crs1', schoolId: 'school-a', branchId: 'b1', categoryId: 'c1', teacherId: 'u1', studentIds: ['u2', 'u9'], title: 'U19 Football Elite',
    location: 'Field A',
    address: '41.0082, 28.9784',
    instructorNotes: 'Please arrive 15 minutes early to start warm-up exercises. Cleats will be checked.',
    schedule: [{ day: today, startTime: '16:00', endTime: '18:00' }, { day: 1, startTime: '16:00', endTime: '18:00' }]
  },
  {
    id: 'crs2', schoolId: 'school-a', branchId: 'b3', categoryId: 'c3', teacherId: 'u7', studentIds: ['u2', 'u9'], title: 'Mathematics Advanced Level',
    location: 'Z-12 Laboratory',
    address: 'Ankara, Cankaya',
    instructorNotes: 'Don\'t forget to bring last week\'s problem set. We will start the derivatives topic.',
    schedule: [{ day: today, startTime: '18:30', endTime: '20:00' }, { day: 2, startTime: '18:30', endTime: '20:00' }]
  }
];

export const MOCK_NOTIFICATIONS: Notification[] = [
  { id: 'n3', type: NotificationType.ANNOUNCEMENT, title: 'Weekly Schedule Update', message: 'Training hours have been rearranged.', timestamp: new Date().toISOString(), isRead: false, senderRole: UserRole.SCHOOL_ADMIN },
  { id: 'n1', type: NotificationType.UPCOMING_CLASS, title: 'Class Starting', message: 'U19 Football Elite Group class starts in 15 minutes.', timestamp: new Date(Date.now() - 1800000).toISOString(), isRead: false },
  { id: 'n2', type: NotificationType.ATTENDANCE_UPDATE, title: 'Attendance Recorded', message: 'Mehmet Kaya was marked as "Present" in today\'s mathematics class.', timestamp: new Date(Date.now() - 3600000).toISOString(), isRead: true }
];

export const DAYS = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
export const SHORT_DAYS = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
