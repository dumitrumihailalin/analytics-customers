export interface WeeklyRevenue {
  week: number;
  year: number;
  totalRevenue: number;
  orderCount: number;
  quantitySold: number;
}

export interface MonthlyRevenue {
  month: number;
  year: number;
  totalRevenue: number;
  orderCount: number;
  quantitySold: number;
}

export interface CategoryBreakdown {
  category: string;
  totalRevenue: number;
  orderCount: number;
  quantitySold: number;
}

export interface DashboardSummary {
  totalRevenue: number;
  totalOrders: number;
  totalQuantitySold: number;
  averageOrderValue: number;
  weeklyRevenue: WeeklyRevenue[];
  monthlyRevenue: MonthlyRevenue[];
  categoryBreakdown: CategoryBreakdown[];
}

export interface AdminStats {
  totalUsers: number;
  activeSubscriptions: number;
  totalOrdersProcessed: number;
  platformTotalRevenue: number;
}

export interface OrderRequest {
  orderReference: string;
  amount: number;
  currency: string;
  status: string;
  productCategory?: string;
  orderDate: string;
}
