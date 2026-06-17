import { useCallback, useEffect, useState } from 'react'
import { getAllSensorsPaged, type AllSensorData } from '../api/client'

interface DashboardState {
  data: AllSensorData | null
  loading: boolean
  error: string | null
  lastUpdated: Date | null
}

export function useDashboardData(autoRefreshMs: number | null = null) {
  const [state, setState] = useState<DashboardState>({
    data: null,
    loading: true,
    error: null,
    lastUpdated: null,
  })

  const load = useCallback(async () => {
    setState((prev) => ({ ...prev, loading: true, error: null }))
    try {
      const data = await getAllSensorsPaged({ page: 1, pageSize: 10 })
      setState({
        data,
        loading: false,
        error: null,
        lastUpdated: new Date(),
      })
    } catch (err) {
      setState((prev) => ({
        ...prev,
        loading: false,
        error: err instanceof Error ? err.message : 'Bilinmeyen hata',
      }))
    }
  }, [])

  useEffect(() => {
    load()
  }, [load])

  useEffect(() => {
    if (!autoRefreshMs) return
    const id = window.setInterval(load, autoRefreshMs)
    return () => window.clearInterval(id)
  }, [autoRefreshMs, load])

  return { ...state, refresh: load }
}
