import React, { useState, useEffect } from 'react'

export default function DescSettings(props) {
  const [data, setData] = useState(props.data)

  useEffect((props) => {
    setData(props.data)
  }, [])

  return(
    <div>
      {JSON.stringify(data)}
    </div>
  )
}