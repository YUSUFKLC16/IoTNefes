import {
  CartesianGrid,
  Legend,
  Line,
  LineChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts'

export interface MultiLineSeries {
  key: string
  label: string
  color: string
  unit: string
}

export interface MultiLineChartProps {
  data: Record<string, string | number>[]
  series: MultiLineSeries[]
  xKey?: string
}

export function MultiLineChart({
  data,
  series,
  xKey = 'time',
}: MultiLineChartProps) {
  return (
    <div className="chart-canvas">
      <ResponsiveContainer width="100%" height="100%">
        <LineChart data={data} margin={{ top: 10, right: 8, left: -10, bottom: 0 }}>
        <CartesianGrid
          stroke="rgba(255,255,255,0.06)"
          strokeDasharray="3 3"
          vertical={false}
        />
        <XAxis
          dataKey={xKey}
          tick={{ fill: 'rgba(255,255,255,0.55)', fontSize: 11 }}
          tickLine={false}
          axisLine={{ stroke: 'rgba(255,255,255,0.08)' }}
        />
        <YAxis
          tick={{ fill: 'rgba(255,255,255,0.55)', fontSize: 11 }}
          tickLine={false}
          axisLine={{ stroke: 'rgba(255,255,255,0.08)' }}
          width={48}
        />
        <Tooltip
          contentStyle={{
            background: 'rgba(17, 19, 28, 0.95)',
            border: '1px solid rgba(255,255,255,0.08)',
            borderRadius: 10,
            fontSize: 12,
            color: '#e5e7eb',
          }}
          labelStyle={{ color: 'rgba(255,255,255,0.55)' }}
        />
        <Legend
          wrapperStyle={{ fontSize: 12, paddingTop: 8 }}
          iconType="circle"
        />
        {series.map((s) => (
          <Line
            key={s.key}
            type="monotone"
            dataKey={s.key}
            name={`${s.label} (${s.unit})`}
            stroke={s.color}
            strokeWidth={2}
            dot={false}
            activeDot={{ r: 4 }}
          />
        ))}
        </LineChart>
      </ResponsiveContainer>
    </div>
  )
}
